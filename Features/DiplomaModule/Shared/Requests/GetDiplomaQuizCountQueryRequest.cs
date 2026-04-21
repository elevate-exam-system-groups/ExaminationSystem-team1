namespace ExaminationSystem.Features.DiplomaModule.Shared.Requests
{
    public record GetDiplomaQuizCountQueryRequest(Guid diplomaID) : IRequest<RequestResult<int>>;


    public class GetDiplomaQuizCountQueryRequestValidator : AbstractValidator<GetDiplomaQuizCountQueryRequest>
    {
        public GetDiplomaQuizCountQueryRequestValidator()
        {
            RuleFor(x => x.diplomaID)
                .NotEmpty().WithMessage("DiplomaId is required");
        }
    }

    public class GetDiplomaQuizCountQueryHandler : IRequestHandler<GetDiplomaQuizCountQueryRequest, RequestResult<int>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        private readonly IValidator<GetDiplomaQuizCountQueryRequest> _validator;
        public GetDiplomaQuizCountQueryHandler(IGeneralRepository<Diploma> diplomaRepository, IValidator<GetDiplomaQuizCountQueryRequest> validator)
        {
            _diplomaRepository = diplomaRepository;
            _validator = validator;
        }
        public async Task<RequestResult<int>> Handle(GetDiplomaQuizCountQueryRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var validationErrors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return RequestResult<int>.Failure(validationErrors, RequestErrorCode.ValidationError);
            }
            var diploma = _diplomaRepository.GetById(request.diplomaID).FirstOrDefault();
            if (diploma is null)
            {
                return RequestResult<int>.Failure("Diploma not found", RequestErrorCode.NotFound);
            }
            var quizCount = diploma.Quizzes.Count;
            return RequestResult<int>.Success(quizCount, "Quiz count retrieved successfully", RequestErrorCode.Success);
        }
    }
}
