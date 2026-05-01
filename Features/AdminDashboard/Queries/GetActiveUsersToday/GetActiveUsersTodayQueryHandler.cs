using ExaminationSystem.Features.AdminDashboard.DTOs;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetActiveUsersToday
{
    public class GetActiveUsersTodayQueryHandler
        : IRequestHandler<GetActiveUsersTodayQuery, RequestResult<ActiveUsersTodayDto>>
    {

        private readonly UserManager<User> _userManager;
        public GetActiveUsersTodayQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RequestResult<ActiveUsersTodayDto>> Handle
            (GetActiveUsersTodayQuery request, CancellationToken ct)
        {

            var count = await _userManager.Users
                .CountAsync(u => u.LastActivityAt.HasValue &&
                        u.LastActivityAt >= DateTime.UtcNow.Date);

            return RequestResult<ActiveUsersTodayDto>.Success(
                new ActiveUsersTodayDto(count));
        }
    }
}
