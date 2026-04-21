namespace ExaminationSystem.Features.Account.Shared
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user);

    }
}
