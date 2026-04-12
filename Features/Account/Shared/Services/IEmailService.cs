namespace ExaminationSystem.Features.Account.Shared.Services
{
    public interface IEmailService
    {
        Task<bool> SendPasswordResetEmailAsync(string email, string token, string userName);
        //Task<bool> SendVerificationEmailAsync(string email, string otp, string userName);

    }
}
