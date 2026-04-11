namespace ExaminationSystem.Domain.Enums
{
    public enum ErrorCode
    {
        Success = 201,
        EmailAlreadyRegistered = 209,
        ValidationError = 422,
        PasswordPolicyViolation = 422
    }
}
