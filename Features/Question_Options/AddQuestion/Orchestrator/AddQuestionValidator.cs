using ExaminationSystem.Features.QuestionFeatures.AddQuestion.Orchestrator;

namespace ExaminationSystem.Features.Question_Options.AddQuestion.Orchestrator
{
    public class AddQuestionValidator
    : AbstractValidator<AddQuestionOrchestratorCommand>
    {
        public AddQuestionValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Question text is required")
                .Length(3, 1000).WithMessage("Question text must be between 3 and 1000 characters");

            RuleFor(x => x.Options)
                .NotNull()
                .Must(o => o.Count >= 2).WithMessage("At least 2 options are required");

            RuleFor(x => x.Options)
                .Must(o => o.Count(opt => opt.IsCorrect) == 1)
                .WithMessage("Exactly one correct option required");

            RuleForEach(x => x.Options)
                .ChildRules(o =>
                {
                    o.RuleFor(x => x.Text)
                     .NotEmpty()
                     .MaximumLength(500);
                });

            RuleFor(x => x.Explanation)
                .MaximumLength(2000).When(x => x.Explanation != null);
        }
    }
}
