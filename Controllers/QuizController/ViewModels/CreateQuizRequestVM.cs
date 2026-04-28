namespace ExaminationSystem.Controllers.QuizController.ViewModels
{
    public record CreateQuizRequestVM
    {
        public string Title { get; init; }
        public Guid DiplomaId { get; init; }
        public int DurationInMinutes { get; init; }
        public decimal PassScore { get; init; }
        public int? MaxAttempts { get; init; }
        public string? Instructions { get; init; }
    }
}
