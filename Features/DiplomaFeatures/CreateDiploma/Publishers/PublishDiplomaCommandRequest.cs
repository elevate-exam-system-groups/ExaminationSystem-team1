using ExaminationSystem.Contracts;
using MassTransit;

namespace ExaminationSystem.Features.DiplomaFeatures.CreateDiploma.Publishers
{
    public record PublishDiplomaCommandRequest(string Title, string? Description) : IRequest<RequestResult<string>> { }

    public class PublishDiplomaCommandHandler(IPublishEndpoint publishEndpoint, ILogger<PublishDiplomaCommandHandler> logger)
        : IRequestHandler<PublishDiplomaCommandRequest, RequestResult<string>>
    {
        public async Task<RequestResult<string>> Handle(PublishDiplomaCommandRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Publishing DiplomaCreated event through IPublishEndpoint.");

            await publishEndpoint.Publish(new DiplomaCreated(
                request.Title,
                request.Description
            ), cancellationToken);

            return RequestResult<string>.Success("تم وضع الطلب في الطابور بنجاح", "Accepted", RequestErrorCode.Success);
        }
    }
}
