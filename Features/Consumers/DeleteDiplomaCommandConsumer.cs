using ExaminationSystem.Contracts.Commands;
using ExaminationSystem.Features.DiplomaFeatures.DeleteDiploma.Commands;
using MassTransit;

namespace ExaminationSystem.Features.Consumers
{
    public class DeleteDiplomaCommandConsumer(IMediator mediator) : IConsumer<DeleteDiplomaCommandMessage>
    {
        public async Task Consume(ConsumeContext<DeleteDiplomaCommandMessage> context)
        {
            var message = context.Message;

            var result = await mediator.Send(
                new DeleteDiplomaCommand(message.DiplomaId),
                context.CancellationToken);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(
                    $"Failed to delete diploma from queue. Reason: {result.Message ?? "Unknown error"}");
            }
        }
    }
}
