using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Admin.Queries.GetActiveUsersToday
{
    public class GetActiveUsersTodayQueryHandler
        : IRequestHandler<GetActiveUsersTodayQuery, RequestResult<ActiveUsersTodayDto>>
    {

        private readonly UserManager<User> _userManager;
        public GetActiveUsersTodayQueryHandler(UserManager<User> userManager)
            => _userManager = userManager;

        public async Task<RequestResult<ActiveUsersTodayDto>> Handle
            (GetActiveUsersTodayQuery request, CancellationToken ct)
        {

            var count = await _userManager.Users
            .Where(u => u.LastActivityAt.HasValue && //===================
                        u.LastActivityAt >= DateTime.UtcNow.Date)
            .CountAsync(ct);

            return RequestResult<ActiveUsersTodayDto>.Success(
                new ActiveUsersTodayDto(count));
        }
    }
}
