using System.ComponentModel.DataAnnotations.Schema;
namespace ExaminationSystem.Domain.Models
{
    public class Question : BaseModel
    {

        [ForeignKey("Quiz")]
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; } = 1;
        public Quiz? Quiz { get; set; }

        public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
        public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

    }
}
