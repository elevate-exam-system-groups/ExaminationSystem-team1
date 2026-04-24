using AutoMapper;
using ExaminationSystem.Features.StartQuizAttempt.Orchestrators.DTOS;
using ExaminationSystem.Features.StartQuizAttempt.Queries.GetQuizAttemptMetaData.DTOS;

namespace ExaminationSystem.Controllers.AttemptController.ViewModels
{
    public record StartAttemptVM { Guid AttemptId; QuizMetaDataVM MetaData; };

    public record QuizMetaDataVM
    {
        Guid DiplomaId;
        string QuizTitle;
        string? QuizInstructions;
        decimal PassScore;
        DateTime DeadLine;
        List<QuestionVM> Questions;
    };

    public record QuestionVM { Guid QuestionId; string Text; int OrderIndex; List<OptionVM> Options; };
    public record OptionVM { Guid OptionId; string Text; };

    public class StartAttemptVMProfile : Profile
    {
        public StartAttemptVMProfile()
        {
            CreateMap<StartAttemptDTO, StartAttemptVM>();
            CreateMap<QuizMetaDataDTO, QuizMetaDataVM>();
            CreateMap<QuestionDTO, QuestionVM>();
            CreateMap<OptionDTO, OptionVM>();
        }
    }
}