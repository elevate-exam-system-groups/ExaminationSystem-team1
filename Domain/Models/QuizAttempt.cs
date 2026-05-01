namespace ExaminationSystem.Domain.Models
{
    public class QuizAttempt : BaseModel
    {
        public string StudentId { get; set; }
        public Guid QuizId { get; set; }
        public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.InProgress;
        public DateTime StartTime { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public decimal? Score { get; set; }
        public bool? IsPassed { get; set; }

        public User Student { get; set; }
        //public Student Student { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<AttemptAnswer> UserAnswers { get; set; } = new List<AttemptAnswer>();

    }
}
