namespace ExaminationSystem.Features.Admin.Queries
{
    public class GetActiveUsersTodayQueryHandler : IRequestHandler<GetActiveUsersTodayQuery, int>
    {
        private readonly IGeneralRepository<User> _userRepo;

        public GetActiveUsersTodayQueryHandler(IGeneralRepository<User> userRepo)
            => _userRepo = userRepo;

        public async Task<int> Handle(GetActiveUsersTodayQuery request, CancellationToken ct)
        {
            var today = DateTime.UtcNow.Date;
            return await _userRepo
                .Get(u => u.LastLoginDate.HasValue
                       && u.LastLoginDate.Value.Date == today
                       && !u.isDeleted)
                .CountAsync(ct);
        }
    }
}
