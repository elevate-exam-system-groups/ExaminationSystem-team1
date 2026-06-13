using ExaminationSystem.Features.AdminDashboard.DTOs;
using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Domain.Data;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetActiveUsersToday
{
    public class GetActiveUsersTodayQueryHandler
        : IRequestHandler<GetActiveUsersTodayQuery, RequestResult<ActiveUsersTodayDto>>
    {

        private readonly Context _context;
        public GetActiveUsersTodayQueryHandler(Context context)
            => _context = context;

        public async Task<RequestResult<ActiveUsersTodayDto>> Handle
            (GetActiveUsersTodayQuery request, CancellationToken ct)
        {

            var count = await _context.Users
            .Where(u => u.LastActivityAt.HasValue && //===================
                        u.LastActivityAt >= DateTime.UtcNow.Date)
            .CountAsync(ct);

            return RequestResult<ActiveUsersTodayDto>.Success(
                new ActiveUsersTodayDto(count));
        }
    }
}
