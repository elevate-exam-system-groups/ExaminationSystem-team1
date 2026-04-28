using ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics.ViewModels;
using ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.DTOs;

namespace ExaminationSystem.Controllers.AdminManagementControllers.PerformanceAnalytics.Mapping
{
    public static class AnalyticsMapper
    {
        public static AnalyticsResponseVm ToViewModel(this AnalyticsResponseDto dto)
        {
            return new AnalyticsResponseVm(
                PassRateByQuiz: dto.PassRateByQuiz?.Select(ToViewModel).ToList() ?? new(),
                AvgScoreByDiploma: dto.AvgScoreByDiploma?.Select(ToViewModel).ToList() ?? new(),
                AttemptsOverTime: dto.AttemptsOverTime?.Select(ToViewModel).ToList() ?? new(),
                TopFailedQuestions: dto.TopFailedQuestions?.Select(ToViewModel).ToList() ?? new()
            );
        }

        private static QuizPassRateVm ToViewModel(this QuizPassRateDto dto)
            => new(dto.QuizId, dto.QuizTitle, dto.PassRate, dto.TotalAttempts);

        private static DiplomaAvgScoreVm ToViewModel(this DiplomaAvgScoreDto dto)
            => new(dto.DiplomaId, dto.DiplomaTitle, dto.AvgScore, dto.TotalAttempts);

        private static AttemptsOverTimeVm ToViewModel(this AttemptsOverTimeDto dto)
            => new(dto.Date, dto.Count);

        private static FailedQuestionVm ToViewModel(this FailedQuestionDto dto)
            => new(dto.QuestionId, dto.QuestionText, dto.FailRate, dto.QuizTitle);
    }
}

