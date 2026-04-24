using ExaminationSystem.Features.Admin.DTOs;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Admin.Queries.GetTotalUsers
{
    public class GetTotalUsersQueryHandler
        : IRequestHandler<GetTotalUsersQuery, RequestResult<TotalUsersDto>>
    {

        private readonly UserManager<User> _userManager;
        public GetTotalUsersQueryHandler(UserManager<User> userManager)
            => _userManager = userManager;

        public async Task<RequestResult<TotalUsersDto>> Handle
            (GetTotalUsersQuery request, CancellationToken ct)
        {
            var count = await _userManager.Users.CountAsync(ct);

            return RequestResult<TotalUsersDto>.Success(
                new TotalUsersDto(count));
        }

    }
}
