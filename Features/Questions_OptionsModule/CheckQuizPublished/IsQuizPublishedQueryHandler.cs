using ExaminationSystem.Features.Questions_OptionsModule.GetQuizStatus;

namespace ExaminationSystem.Features.Questions_OptionsModule.CheckQuizPublished
{
    public class IsQuizPublishedQueryHandler : IRequestHandler<IsQuizPublishedQuery, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        public IsQuizPublishedQueryHandler(IMediator mediator)
            => _mediator = mediator;

        public async Task<RequestResult<bool>> Handle(IsQuizPublishedQuery request, CancellationToken ct)
        {
            var statusResult = await _mediator.Send(new GetQuizStatusQuery(request.QuizId), ct);

            if (!statusResult.IsSuccess)
                return RequestResult<bool>.Failure(statusResult.Message);

            return RequestResult<bool>.Success(statusResult.Data == QuizStatus.Published);
        }
    }
}
