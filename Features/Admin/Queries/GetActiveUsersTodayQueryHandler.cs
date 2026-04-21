using ExaminationSystem.Features.Admin.DTOs;

namespace ExaminationSystem.Features.Admin.Queries
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
            var today = DateTime.UtcNow.Date;
            var count = await _userManager.Users
                .Where(u => u.LastLoginAt.HasValue 
                       && u.LastLoginAt.Value.Date == today)
                .CountAsync(ct);

            return RequestResult<ActiveUsersTodayDto>.Success(new ActiveUsersTodayDto(count));
        }
    }
}
