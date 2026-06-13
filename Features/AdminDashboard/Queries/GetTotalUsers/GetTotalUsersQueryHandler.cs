using ExaminationSystem.Features.AdminDashboard.DTOs;
using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Domain.Data;

namespace ExaminationSystem.Features.AdminDashboard.Queries.GetTotalUsers
{
    public class GetTotalUsersQueryHandler
        : IRequestHandler<GetTotalUsersQuery, RequestResult<TotalUsersDto>>
    {

        private readonly Context _context;
        public GetTotalUsersQueryHandler(Context context)
            => _context = context;

        public async Task<RequestResult<TotalUsersDto>> Handle
            (GetTotalUsersQuery request, CancellationToken ct)
        {
            var count = await _context.Users.CountAsync(ct);

            return RequestResult<TotalUsersDto>.Success(
                new TotalUsersDto(count));
        }

    }
}
