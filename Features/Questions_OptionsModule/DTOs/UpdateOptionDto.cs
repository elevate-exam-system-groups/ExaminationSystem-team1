namespace ExaminationSystem.Features.Questions_OptionsModule.DTOs
{
    public record UpdateOptionDto(Guid? Id , string Text , bool IsCorrect);
}
