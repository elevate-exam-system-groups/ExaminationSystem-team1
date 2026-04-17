namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands
{
    public class CreateQuestionCommandHandler
    : IRequestHandler<CreateQuestionCommand, RequestResult<CreateQuestionResponse>>
    {
        private readonly IGeneralRepository<Question> _questionRepo;

        public CreateQuestionCommandHandler(IGeneralRepository<Question> questionRepo)
            => _questionRepo = questionRepo;

        public async Task<RequestResult<CreateQuestionResponse>> Handle(
            CreateQuestionCommand request,
            CancellationToken ct)
        {
            var question = new Question
            {
                Id = Guid.NewGuid(),
                QuizId = request.QuizId,
                Text = request.Text,
                Explanation = request.Explanation,
                OrderIndex = request.OrderIndex,
                CreatedAt = DateTime.UtcNow
            };

            _questionRepo.Add(question);

            return RequestResult<CreateQuestionResponse>.Success(
                new CreateQuestionResponse(question.Id));
        }
    }
}
