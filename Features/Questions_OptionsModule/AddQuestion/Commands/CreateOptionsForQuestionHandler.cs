namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands
{
    public class CreateOptionsForQuestionHandler
        : IRequestHandler<CreateOptionsForQuestionCommand, RequestResult<CreateOptionsForQuestionResponse>>
    {

        private readonly IGeneralRepository<Option> _optionRepo;
        public CreateOptionsForQuestionHandler(IGeneralRepository<Option> optionRepo)
            => _optionRepo = optionRepo;

        public async Task<RequestResult<CreateOptionsForQuestionResponse>> Handle(
            CreateOptionsForQuestionCommand request,
            CancellationToken ct)
        {
            foreach (var opt in request.Options)
            {
                _optionRepo.Add(new Option
                {
                    Id = Guid.NewGuid(),
                    QuestionId = request.QuestionId,
                    Text = opt.Text,
                    IsCorrect = opt.IsCorrect,
                    CreatedAt = DateTime.UtcNow
                });
            }

            return RequestResult<CreateOptionsForQuestionResponse>.Success(
                new CreateOptionsForQuestionResponse(request.Options.Count));
        }
    }
}