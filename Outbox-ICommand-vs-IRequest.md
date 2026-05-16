# Transactional Outbox: `ICommand` vs `IRequest` in this project

## What was changed

1. `PublishDiplomaCommandRequest` now implements `ICommand<RequestResult<string>>` instead of plain `IRequest<RequestResult<string>>`.
2. `TransactionMiddleware` now calls `await context.SaveChangesAsync();` before commit.
3. `TransactionMiddleware` is now enabled in `Program.cs`.

## Difference between `ICommand<T>` and `IRequest<T>`

- `IRequest<T>` is the generic MediatR contract for any request (query, command, etc.).
- `ICommand<T>` in this codebase is a marker interface that inherits from `IRequest<T>`.
- The key value is behavior selection: your `TransactionBehavior<TRequest, TResponse>` is constrained to:

```csharp
where TRequest : ICommand<TResponse>
```

So only requests marked as `ICommand<T>` go through that MediatR transaction behavior automatically.

## Why outbox save did not work with `IRequest` earlier

In your previous state:

- `PublishDiplomaCommandRequest` was `IRequest<T>`, so it did **not** pass through `TransactionBehavior`.
- `TransactionMiddleware` was disabled in `Program.cs`.
- `context.SaveChangesAsync()` inside middleware was commented.

With that combination, there was no active persistence point to flush MassTransit bus outbox records into the database table.

## Important note when middleware is enabled

If middleware is enabled and `SaveChangesAsync()` is called, outbox messages can still be persisted for non-GET HTTP flows even for `IRequest`.

However, using `ICommand` is still preferred here because it gives explicit application-layer intent and guarantees transaction behavior through MediatR pipeline (not only through HTTP middleware).
