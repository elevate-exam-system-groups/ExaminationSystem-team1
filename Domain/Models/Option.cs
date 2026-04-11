namespace ExaminationSystem.Domain.Models
{
    public class Option : BaseModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // Navigation property
        public Question Question { get; set; }

        public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

    }
}
