namespace ExaminationSystem.Controllers.QuestionController.ViewModels
{
    public record UpdateQuestionVM(string Text ,string? Explanation , List<UpdateOptionVM> Options);
    
}
