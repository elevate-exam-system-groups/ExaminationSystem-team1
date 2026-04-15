namespace ExaminationSystem.Features.Common.Enums
{
    public enum RequestErrorCode
    {
        None,
        InvalidCredentials,
        EmailNotVerified,
        AccountLocked,
        AccountNotActive,

        Success = 201,
        EmailAlreadyRegistered = 209,
        ValidationError = 422,
        PasswordPolicyViolation = 422,

        UserNotFound ,
        InvalidToken ,
        PasswordResetFailed,
        EmailSendFailed,
        EmailAlreadyExists,
        AccountNotVerified,
        Conflict = 409
    }
}
