using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Domain.Models
{
    public class QuestionOption : BaseModel
    {
        [ForeignKey("Question")]
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public Question Question { get; set; }

        public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

    }
}
