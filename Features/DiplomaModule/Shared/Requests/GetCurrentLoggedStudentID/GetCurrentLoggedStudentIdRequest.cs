using System.Security.Claims;

namespace ExaminationSystem.Features.DiplomaModule.Shared.Requests.GetCurrentLoggedStudentID
{
    public record GetCurrentLoggedStudentIdRequest() : IRequest<RequestResult<string>>;

    public class GetCurrentLoggedStudentIdRequestHandler : IRequestHandler<GetCurrentLoggedStudentIdRequest, RequestResult<string>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCurrentLoggedStudentIdRequestHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<RequestResult<string>> Handle(GetCurrentLoggedStudentIdRequest request, CancellationToken cancellationToken)
        {
            var studentId = _httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.NameIdentifier);

            return Task.FromResult(RequestResult<string>
                .Success(studentId, "Current logged student ID retrieved successfully", RequestErrorCode.Success));
        }
    }
}
