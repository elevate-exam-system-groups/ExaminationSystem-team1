namespace ExaminationSystem.Features.AuthModule.UserLogin.LoginDTOS
{
    public class LoginCommandResponseDTO
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
