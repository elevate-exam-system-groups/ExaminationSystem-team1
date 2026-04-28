namespace ExaminationSystem.Features.Common.DiplomaRequests.Queries
{
    public record IsDiplomaExistQuery(Guid DiplomaId) : IRequest<RequestResult<bool>>;

    public class IsDiplomaExistQueryHandler : IRequestHandler<IsDiplomaExistQuery, RequestResult<bool>>
    {
        private readonly IGeneralRepository<Diploma> _diplomaRepository;
        public IsDiplomaExistQueryHandler(IGeneralRepository<Diploma> diplomaRepository)
        {
            _diplomaRepository = diplomaRepository;
        }
        public async Task<RequestResult<bool>> Handle(IsDiplomaExistQuery request, CancellationToken cancellationToken)
        {
            var isExist = _diplomaRepository.GetById(request.DiplomaId).Any();

            if (!isExist)
                return RequestResult<bool>.Failure("Diploma not found", RequestErrorCode.NotFound);

            return RequestResult<bool>.Success(isExist);
        }
    }

}
