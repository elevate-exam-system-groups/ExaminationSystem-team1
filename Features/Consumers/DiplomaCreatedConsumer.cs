using ExaminationSystem.Contracts;
using ExaminationSystem.Features.DiplomaFeatures.CreateDiploma.Commands;
using MassTransit;

namespace ExaminationSystem.Features.Consumers
{
    public class DiplomaCreatedConsumer(IMediator mediator) : IConsumer<DiplomaCreated>
    {
        public async Task Consume(ConsumeContext<DiplomaCreated> context)
        {
            var message = context.Message;
            var result = await mediator.Send(
                new CreateDiplomaCommandRequest(message.Title, message.Description),
                context.CancellationToken);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(
                    $"Failed to persist diploma from queue. Reason: {result.Message ?? "Unknown error"}");
            }
        }
    }
}
