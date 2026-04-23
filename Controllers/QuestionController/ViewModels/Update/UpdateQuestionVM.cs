namespace ExaminationSystem.Controllers.QuestionController.ViewModels.Update
{
    public record UpdateQuestionVM(string Text, string? Explanation, List<UpdateOptionVM> Options);

}
