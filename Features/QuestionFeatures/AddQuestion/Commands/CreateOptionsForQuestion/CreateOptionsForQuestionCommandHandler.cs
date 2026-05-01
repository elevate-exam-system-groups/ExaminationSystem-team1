using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.QuestionFeatures.AddQuestion.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace ExaminationSystem.Features.QuestionFeatures.AddQuestion.Commands.CreateOptionsForQuestion
{
    public class CreateOptionsForQuestionCommandHandler
        : IRequestHandler<CreateOptionsForQuestionCommand, RequestResult<CreateOptionsResponseDto>>
    {

        private readonly IGeneralRepository<QuestionOption> _optionRepo;
        public CreateOptionsForQuestionCommandHandler(IGeneralRepository<QuestionOption> optionRepo)
            => _optionRepo = optionRepo;

        public async Task<RequestResult<CreateOptionsResponseDto>> Handle(
        CreateOptionsForQuestionCommand request, CancellationToken ct)
        {
            foreach (var opt in request.Options)
            {
                _optionRepo.Add(new QuestionOption
                {
                    QuestionId = request.QuestionId,
                    Text = opt.Text,
                    IsCorrect = opt.IsCorrect
                });
            }

            await _optionRepo.SaveChangesAsync();

            //  Whats the best practice for returning success response when we have no data to return?
            //  should we return a [IDs only] or a [boolean] that success returned?
            return RequestResult<CreateOptionsResponseDto>.Success
                (new CreateOptionsResponseDto(true) , "Options created successfully");
        }
    }
}