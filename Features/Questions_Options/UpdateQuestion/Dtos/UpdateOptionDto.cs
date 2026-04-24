namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Dtos
{
    public record UpdateOptionDto(Guid? Id, string Text, bool IsCorrect);

}
