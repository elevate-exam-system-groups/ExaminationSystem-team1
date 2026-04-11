namespace ExaminationSystem.Domain.Models
{
    public class AttemptAnswer : BaseModel
    {
        public int QuizAttemptId { get; set; }
        public int OptionId { get; set; }
        public DateTime AnsweredAt { get; set; }

        // Navigation properties
        public QuizAttempt QuizAttempt { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public Option Option { get; set; }

    }
}
