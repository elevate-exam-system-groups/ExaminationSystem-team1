using System.Security.Claims;

namespace ExaminationSystem.Features.Common.Helpers
{
    public class UserContextService : IUserContextService
    {
        public string UserId { get; private set; }

        public UserContextService(IHttpContextAccessor accessor)
        {
            var UserId = accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
