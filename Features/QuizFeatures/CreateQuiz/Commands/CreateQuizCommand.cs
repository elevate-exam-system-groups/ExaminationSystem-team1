
namespace ExaminationSystem.Features.QuizFeatures.CreateQuiz.Commands
{
    public record CreateQuizCommand(
        string Title,
        Guid DiplomaId,
        int DurationInMinutes,
        decimal PassScore,
        int? MaxAttempts,
        string? Instructions) : IRequest<RequestResult<Guid>>;

    public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;

        public CreateQuizCommandValidator(IGeneralRepository<Diploma> diplomaRepository)
        {
            _diplomaRepository = diplomaRepository;

            RuleFor(x => x.DiplomaId)
                .NotEmpty().WithMessage("Diploma ID cannot be empty.")
                .MustAsync(async (diplomaId, cancellationToken) =>
                {
                    return await _diplomaRepository.GetById(diplomaId).AnyAsync(cancellationToken);
                }).WithMessage("Diploma not found.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Quiz title cannot be empty.");
            RuleFor(x => x.DurationInMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than zero.");
            RuleFor(x => x.PassScore)
                .GreaterThan(0).WithMessage("Pass score must be greater than zero.")
                .LessThanOrEqualTo(100).WithMessage("Pass score must be between 0 and 100.");
        }
    }

    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, RequestResult<Guid>>
    {
        private readonly IGeneralRepository<Quiz> _quizRepository;
        private readonly IValidator<CreateQuizCommand> _validator;

        public CreateQuizCommandHandler(IGeneralRepository<Quiz> quizRepository, IValidator<CreateQuizCommand> validator)
        {
            _quizRepository = quizRepository;
            _validator = validator;
        }
        public async Task<RequestResult<Guid>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            //var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            //if (!validationResult.IsValid)
            //{
            //    return RequestResult<Guid>.Failure("Validation failed.", RequestErrorCode.ValidationError);
            //}


            Quiz? NewQuiz = new Quiz
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                DiplomaId = request.DiplomaId,
                DurationInMinutes = request.DurationInMinutes,
                PassScore = request.PassScore,
                MaxAttempts = request.MaxAttempts,
                Instructions = request.Instructions,
                Status = QuizStatus.Draft
            };


            Guid CreatedQuizId = _quizRepository.AddAndReturnId(NewQuiz);
            await _quizRepository.SaveChangesAsync();

            return RequestResult<Guid>.Success(CreatedQuizId);
        }
    }

}
