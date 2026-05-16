# MassTransit + RabbitMQ Flow in ExaminationSystem

This document explains the current messaging implementation in this project from package setup to runtime flow, including the **fanout (event)** and **direct command (point-to-point send)** patterns.

---

## 1. Packages and setup

The project uses:

- `MassTransit` `9.1.0`
- `MassTransit.RabbitMQ` `9.1.0`

From `ExaminationSystem.csproj`:

```xml
<PackageReference Include="MassTransit" Version="9.1.0" />
<PackageReference Include="MassTransit.RabbitMQ" Version="9.1.0" />
```

If you need to install manually:

```bash
dotnet add package MassTransit --version 9.1.0
dotnet add package MassTransit.RabbitMQ --version 9.1.0
```

---

## 2. DI and broker configuration

MassTransit is configured in `Program.cs`:

- RabbitMQ host: `localhost` with `guest/guest`
- Registered consumers:
  - `DiplomaCreatedConsumer`
  - `DeleteDiplomaCommandConsumer`
- Receive endpoints:
  - `Diploma-creation-queue`
  - `diploma-delete-command-queue`
- Retry + outbox are enabled on both endpoints:
  - `UseMessageRetry(1s, 5s, 10s)`
  - `UseInMemoryOutbox()`

This gives resilient processing and pushes failed messages to error/dead-letter paths after retries.

---

## 3. Pattern A — Fanout Event (Publish)

## Purpose
Use when a business event can be consumed by one or many subscribers.

## Implemented for
**Create Diploma v2 async flow**.

### Contract
- `Contracts/DiplomaCreated.cs`
- Positional record:

```csharp
public record DiplomaCreated(string Title, string? Description);
```

### Publisher (MediatR handler)
- `Features/DiplomaFeatures/CreateDiploma/Publishers/PublishDiplomaCommandRequest.cs`
- Uses `IPublishEndpoint.Publish(...)`.

### Consumer
- `Features/Consumers/DiplomaCreatedConsumer.cs`
- Implements `IConsumer<DiplomaCreated>`.
- Calls application command `CreateDiplomaCommandRequest`.
- Throws if command fails, so retry/dead-letter policies can apply.

### Endpoint
- `POST /api/v2/Diploma/CreateDiploma`
- `Features/DiplomaFeatures/CreateDiploma/CreateDiplomaEndpoint.cs`
- Sends MediatR request `PublishDiplomaCommandRequest`.

### Runtime flow
1. API receives create request.
2. Handler publishes `DiplomaCreated`.
3. Message is routed to `Diploma-creation-queue` topology.
4. `DiplomaCreatedConsumer` consumes it.
5. Consumer executes create command and persists diploma.

---

## 4. Pattern B — Direct Command (Point-to-Point Send)

## Purpose
Use when exactly one specific worker must process a command.

## Implemented for
**Delete Diploma async command flow**.

### Contract
- `Contracts/Commands/DeleteDiplomaCommandMessage.cs`

```csharp
public record DeleteDiplomaCommandMessage(Guid DiplomaId);
```

### Sender (MediatR handler)
- `Features/DiplomaFeatures/DeleteDiploma/Senders/SendDeleteDiplomaCommandRequest.cs`
- Uses `ISendEndpointProvider`.
- Gets explicit queue endpoint:

```csharp
var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:diploma-delete-command-queue"));
await endpoint.Send(new DeleteDiplomaCommandMessage(request.DiplomaId), cancellationToken);
```

### Consumer
- `Features/Consumers/DeleteDiplomaCommandConsumer.cs`
- Implements `IConsumer<DeleteDiplomaCommandMessage>`.
- Calls domain/app command `DeleteDiplomaCommand`.
- Throws on failure so retry/dead-letter applies.

### Endpoint
- `DELETE /api/v2/Diploma/DeleteDiploma?diplomaID=...`
- `Features/DiplomaFeatures/DeleteDiploma/DeleteDiplomaEndpoint.cs`
- Sends MediatR request `SendDeleteDiplomaCommandRequest`.

### Runtime flow
1. API receives delete request.
2. Sender resolves queue `diploma-delete-command-queue`.
3. Sender sends `DeleteDiplomaCommandMessage` directly to that queue endpoint.
4. `DeleteDiplomaCommandConsumer` processes it.
5. Consumer executes delete logic (`SoftDelete + SaveChanges` through existing command handler).

---

## 5. Fanout vs Direct in this project

## Fanout (event-driven)
- Used for `DiplomaCreated` via `Publish`.
- Semantics: broadcast-style event.
- Best for integration events and future multiple subscribers.

## Direct command (point-to-point)
- Used for `DeleteDiplomaCommandMessage` via queue `Send`.
- Semantics: one queue/one worker responsibility.
- Best for command processing where you target a specific worker path.

> Note: RabbitMQ UI may still show endpoint exchanges created by MassTransit for receive endpoints.  
> The important behavior is:
> - Create uses **Publish event flow**.
> - Delete uses **Send to a specific queue** (command flow).

---

## 6. Failure handling strategy

Both consumers are configured with:

- Retry intervals: `1s`, `5s`, `10s`
- In-memory outbox
- Exception throwing on command failure

So processing behavior is:

1. Consumer receives message
2. Business command fails -> consumer throws
3. MassTransit retries
4. If still failing, message is moved to error/dead-letter path

---

## 7. Where to extend from here

- Keep **Publish** for domain/integration events (`DiplomaCreated` style).
- Keep **Send** for strict command workers (`DeleteDiplomaCommandMessage` style).
- Add more command contracts under `Contracts/Commands` and dedicated consumers per command.
