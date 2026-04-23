namespace ExaminationSystem.Controllers.QuestionController.ViewModels
{
    public record AddQuestionVM(string Text ,string? Explanation ,List<OptionViewModel> Options);
}


