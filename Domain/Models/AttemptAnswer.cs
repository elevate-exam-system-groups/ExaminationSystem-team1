using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Domain.Models
{
    public class AttemptAnswer : BaseModel
    {
        public Guid QuizAttemptId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid SelectedOptionId { get; set; }
        public DateTime AnsweredAt { get; set; }
        public bool? IsCorrect { get; set; }
        public QuizAttempt QuizAttempt { get; set; }
        public Question Question { get; set; }
        public QuestionOption SelectedOption { get; set; }

    }

    
}
