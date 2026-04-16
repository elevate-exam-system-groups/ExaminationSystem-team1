namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.DTOs
{
    public record UpdateOptionDto(Guid? Id, string Text, bool IsCorrect);
}
