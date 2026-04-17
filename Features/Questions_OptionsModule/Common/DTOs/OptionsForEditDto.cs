namespace ExaminationSystem.Features.Questions_OptionsModule.Common.DTOs
{
    public record OptionsForEditDto(Guid QuestionId, List<OptionForEditDto> Options);
}
