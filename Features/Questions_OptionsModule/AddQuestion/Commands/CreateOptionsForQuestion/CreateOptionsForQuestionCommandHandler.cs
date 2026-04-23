using System.Diagnostics.CodeAnalysis;

namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Commands.CreateOptionsForQuestion
{
    public class CreateOptionsForQuestionCommandHandler
        : IRequestHandler<CreateOptionsForQuestionCommand, RequestResult<CreateOptionsResponse>>
    {

        private readonly IGeneralRepository<Option> _optionRepo;
        public CreateOptionsForQuestionCommandHandler(IGeneralRepository<Option> optionRepo)
            => _optionRepo = optionRepo;

        public async Task<RequestResult<CreateOptionsResponse>> Handle(
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

            await _optionRepo.SaveChangesAsync();

            //  Whats the best practice for returning success response when we have no data to return?
            //  should we return a [IDs only] or a [boolean] that success returned?
            return RequestResult<CreateOptionsResponse>.Success
                (new CreateOptionsResponse(true) , "Options created successfully");
        }
    }
}