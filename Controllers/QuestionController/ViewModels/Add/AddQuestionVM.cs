namespace ExaminationSystem.Controllers.QuestionController.ViewModels.Add
{
    public record AddQuestionVM(string Text, string? Explanation, List<OptionVM> Options);
}


