using ExaminationSystem.Contracts;
using ExaminationSystem.Domain.Data;
using MassTransit;

namespace ExaminationSystem.Features.DiplomaFeatures.CreateDiploma.Publishers
{
    public record PublishDiplomaCommandRequest(string Title, string? Description) : IRequest<RequestResult<string>> { }

    public class PublishDiplomaCommandHandler(
        IPublishEndpoint publishEndpoint,
        Context context,
        ILogger<PublishDiplomaCommandHandler> logger)
        : IRequestHandler<PublishDiplomaCommandRequest, RequestResult<string>>
    {
        public async Task<RequestResult<string>> Handle(PublishDiplomaCommandRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting DB transaction for diploma publish request. Title: {Title}", request.Title);
            await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Publishing DiplomaCreated event through IPublishEndpoint. Message will be stored in EF Outbox before broker delivery.");
            await publishEndpoint.Publish(new DiplomaCreated(
                request.Title,
                request.Description
            ), cancellationToken);

            logger.LogInformation("Flushing EF Core changes so MassTransit can persist the outbox message in OutboxMessage table.");
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Committing DB transaction. Outbox message becomes eligible for background delivery to RabbitMQ after commit.");
            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("DB transaction committed successfully for diploma publish request. Title: {Title}", request.Title);
            return RequestResult<string>.Success("تم وضع الطلب في الطابور بنجاح", "Accepted", RequestErrorCode.Success);
        }
    }
}
