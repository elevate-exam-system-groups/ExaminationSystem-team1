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
        {
            _optionRepo = optionRepo;
        }

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

            return RequestResult<CreateOptionsResponseDto>.Success
                (new CreateOptionsResponseDto(true) , "Options created successfully");
        }
    }
}