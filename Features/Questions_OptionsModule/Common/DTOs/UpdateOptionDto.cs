namespace ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs
{
    public record UpdateOptionDto(Guid? Id, string Text, bool IsCorrect);
}
