using AutoMapper;
using ExaminationSystem.Features.GetDiplomaWithQuizzesForLoggedStudent.Orchestrators.DTOS;

namespace ExaminationSystem.Controllers.DiplomaController.ViewModels
{
    public record GetDiplomaQuizzesForLoggedStudenResponseVM
    {
        public Guid QuizId { get; init; }
        public string QuizTitle { get; init; }
        public int AttemptsCount { get; init; }
        public int DurationInMinutes { get; init; }
        public decimal PassScore { get; init; }
        public int? MaxAttempts { get; init; }
        public bool CanAttempt { get; init; }
        public decimal? LastScore { get; init; }
        public QuizStatus Status { get; init; }
    }

    public class ViewDiplomaQuizzesResponseVMProfile : Profile
    {
        public ViewDiplomaQuizzesResponseVMProfile()
        {
            CreateMap<GetDiplomaQuizzesForLoggedStudentDTO, GetDiplomaQuizzesForLoggedStudenResponseVM>();
        }
    }
}
