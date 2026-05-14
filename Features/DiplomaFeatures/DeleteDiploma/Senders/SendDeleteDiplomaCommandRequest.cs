using ExaminationSystem.Contracts.Commands;
using MassTransit;

namespace ExaminationSystem.Features.DiplomaFeatures.DeleteDiploma.Senders
{
    public record SendDeleteDiplomaCommandRequest(Guid DiplomaId) : IRequest<RequestResult<string>>;

    public class SendDeleteDiplomaCommandHandler(ISendEndpointProvider sendEndpointProvider)
        : IRequestHandler<SendDeleteDiplomaCommandRequest, RequestResult<string>>
    {
        public async Task<RequestResult<string>> Handle(SendDeleteDiplomaCommandRequest request, CancellationToken cancellationToken)
        {
            await sendEndpointProvider.Send(new DeleteDiplomaCommandMessage(request.DiplomaId), cancellationToken);

            return RequestResult<string>.Success("Delete command queued successfully", "Accepted", RequestErrorCode.Success);
        }
    }
}
