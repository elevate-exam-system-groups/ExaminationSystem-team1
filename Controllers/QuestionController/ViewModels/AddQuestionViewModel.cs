namespace ExaminationSystem.Controllers.QuestionController.ViewModels
{
    public class AddQuestionViewModel
    {
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<OptionViewModel> Options { get; set; }
    }

}
