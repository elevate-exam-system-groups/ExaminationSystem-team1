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
        UpdateSuccess = 200,
        EmailAlreadyRegistered = 209,
        Forbidden = 403,
        ValidationError = 422,
        PasswordPolicyViolation = 422,
        NotFound = 404,
        Conflict = 409,
        UserNotFound = 1001,
        InvalidToken = 1002,
        PasswordResetFailed = 1003,
        EmailSendFailed = 1004,
        EmailAlreadyExists = 1005,
        AccountNotVerified = 1006
    }
}
