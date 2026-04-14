namespace ExaminationSystem.Controllers.QuestionController.ViewModels
{
    public class UpdateQuestionViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public List<UpdateOptionViewModel> Options { get; set; }
    }
}
