namespace ExaminationSystem.Features.AuthModule.Shared
{
    public interface ITokenGenerator
    {
        string GenerateAccessToken(User user);

    }
}
