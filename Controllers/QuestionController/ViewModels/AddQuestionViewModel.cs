namespace ExaminationSystem.Controllers.QuestionController.ViewModels
{
    public class AddQuestionViewModel
    {
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<OptionViewModel> Options { get; set; }
    }

}
