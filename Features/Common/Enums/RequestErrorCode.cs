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
        InternalServerError = 500,

        UserNotFound ,
        InvalidToken ,
        PasswordResetFailed,
        EmailSendFailed,
        EmailAlreadyExists,
        AccountNotVerified 
    }
}
