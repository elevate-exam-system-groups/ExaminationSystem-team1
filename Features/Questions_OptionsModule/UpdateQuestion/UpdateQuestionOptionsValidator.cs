namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion
{
    public class UpdateQuestionOptionsValidator : AbstractValidator<UpdateQuestionOptionsOrchestrator>
    {
        public UpdateQuestionOptionsValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Text).NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.Options)
                .Must(x => x != null && x.Count >= 2)
                .WithMessage("At least 2 options are required.");

            RuleFor(x => x.Options)
                .Must(x => x != null && x.Count(o => o.IsCorrect) == 1)
                .WithMessage("Exactly one correct option required.");

            RuleForEach(x => x.Options).ChildRules(option =>
            {
                option.RuleFor(o => o.Text)
                .NotEmpty()
                .WithMessage("Option text cannot be empty");
            });
        }
    }
}
