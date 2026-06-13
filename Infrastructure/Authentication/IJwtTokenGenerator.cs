using ExaminationSystem.Domain.Models;

namespace ExaminationSystem.Infrastructure.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, string role);
    }
}
