using System.ComponentModel.DataAnnotations.Schema;
﻿namespace ExaminationSystem.Domain.Models
{
    public class Question : BaseModel
    {
        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; }

        // Navigation property
        public Quiz? Quiz { get; set; }
        public ICollection<Option> Options { get; set; } = new List<Option>();

        public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

    }
}
