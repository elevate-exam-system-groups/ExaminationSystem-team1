namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateQuestion
{
    public class CreateQuestionCommandHandler
    : IRequestHandler<CreateQuestionCommand, RequestResult<CreateQuestionResponseDto>>
    {

        private readonly IGeneralRepository<Question> _questionRepo;
        public CreateQuestionCommandHandler(IGeneralRepository<Question> questionRepo)
            => _questionRepo = questionRepo;

        public async Task<RequestResult<CreateQuestionResponseDto>> Handle(
            CreateQuestionCommand request, CancellationToken ct)
        {
            ///===============================
            var question = new Question
            {
                QuizId = request.QuizId,
                Text = request.Text,
                Explanation = request.Explanation,
                OrderIndex = request.OrderIndex,
            };

            _questionRepo.Add(question);

            await _questionRepo.SaveChangesAsync();

            return RequestResult<CreateQuestionResponseDto>.Success(
                new CreateQuestionResponseDto(question.Id), "Question Created.");
        }
    }
}
