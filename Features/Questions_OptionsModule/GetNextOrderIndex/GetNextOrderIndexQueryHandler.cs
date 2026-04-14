using ExaminationSystem.Features.Questions_OptionsModule.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.GetNextOrderIndex
{
    public class GetNextOrderIndexQueryHandler : IRequestHandler<GetNextOrderIndexQuery, RequestResult<GetNextOrderIndexResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNextOrderIndexQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<GetNextOrderIndexResponse>> Handle(
            GetNextOrderIndexQuery request, CancellationToken cancellationToken)
        {
            var questionRepo = _unitOfWork.GetRepository<Question>();

            var maxOrder = await questionRepo
                .Get(q => q.QuizId == request.QuizId)
                .MaxAsync(q => (int?)q.OrderIndex, cancellationToken) ?? 0;

            return RequestResult<GetNextOrderIndexResponse>.Success(
                new GetNextOrderIndexResponse(maxOrder + 1));
            
        }
    }

}
