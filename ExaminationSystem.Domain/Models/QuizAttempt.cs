namespace ExaminationSystem.Domain.Models
{
    public class QuizAttempt : BaseModel
    {
        public int StudentId { get; set; }
        public int QuizId { get; set; }

        public string status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int Score { get; set; }
        public bool IsPassed { get; set; }
        // Navigation properties
        public User Student { get; set; }
        public Quiz Quiz { get; set; }

    }
}
