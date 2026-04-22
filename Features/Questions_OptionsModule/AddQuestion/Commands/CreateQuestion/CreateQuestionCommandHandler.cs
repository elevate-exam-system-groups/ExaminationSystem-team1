namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateQuestion
{
    public class CreateQuestionCommandHandler
    : IRequestHandler<CreateQuestionCommand, RequestResult<CreateQuestionResponse>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public CreateQuestionCommandHandler(IGeneralRepository<Question> questionRepo)
            => _questionRepo = questionRepo;

        public async Task<RequestResult<CreateQuestionResponse>> Handle(
            CreateQuestionCommand request, CancellationToken ct)
        {
            var question = new Question
            {
                QuizId = request.QuizId,
                Text = request.Text,
                Explanation = request.Explanation,
                OrderIndex = request.OrderIndex,
            };

            _questionRepo.Add(question);

            await _questionRepo.SaveChangesAsync();

            return RequestResult<CreateQuestionResponse>.Success(
                new CreateQuestionResponse(question.Id), "Question Created.");
        }
    }
}
