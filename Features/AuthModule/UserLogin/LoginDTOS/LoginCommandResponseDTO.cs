namespace ExaminationSystem.Features.AuthModule.UserLogin.LoginDTOS
{
    public class LoginCommandResponseDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
