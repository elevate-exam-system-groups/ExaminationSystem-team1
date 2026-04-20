namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateOptionsForQuestion
{
    public class CreateOptionsForQuestionHandler
        : IRequestHandler<CreateOptionsForQuestionCommand, RequestResult<bool>>
    {

        private readonly IGeneralRepository<Option> _optionRepo;
        public CreateOptionsForQuestionHandler(IGeneralRepository<Option> optionRepo)
            => _optionRepo = optionRepo;

        public async Task<RequestResult<bool>> Handle(
       CreateOptionsForQuestionCommand request, CancellationToken ct)
        {
            foreach (var opt in request.Options)
            {
                _optionRepo.Add(new Option
                {
                    QuestionId = request.QuestionId,
                    Text = opt.Text,
                    IsCorrect = opt.IsCorrect
                });
            }

            return RequestResult<bool>.Success(true, "Options created successfully");
        }
    }
}