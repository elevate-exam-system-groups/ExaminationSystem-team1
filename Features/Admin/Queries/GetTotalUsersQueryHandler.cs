namespace ExaminationSystem.Features.Admin.Queries
{
    public class GetTotalUsersQueryHandler : IRequestHandler<GetTotalUsersQuery, int>
    {
        private readonly IGeneralRepository<User> _userRepo;
        public GetTotalUsersQueryHandler(IGeneralRepository<User> userRepo)
            => _userRepo = userRepo;

        public async Task<int> Handle(GetTotalUsersQuery request, CancellationToken ct)
            => await _userRepo.Get(u => !u.isDeleted).CountAsync(ct);
    }
}
