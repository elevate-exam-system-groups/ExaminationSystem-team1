namespace ExaminationSystem.Features.Admin.Queries
{
    public class GetTotalUsersQueryHandler 
        : IRequestHandler<GetTotalUsersQuery, RequestResult<int>>
    {

        private readonly UserManager<User> _userManager;
        public GetTotalUsersQueryHandler(UserManager<User> userManager) 
            => _userManager = userManager;

        public async Task<RequestResult<int>> Handle
            (GetTotalUsersQuery request, CancellationToken ct)
        {
            var count = await _userManager.Users.CountAsync(ct);
            return RequestResult<int>.Success(count);
        }

    }
}
