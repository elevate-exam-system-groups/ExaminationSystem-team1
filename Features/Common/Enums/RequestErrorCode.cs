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
        NotFound = 404,
        Gone = 410,
        Conflict = 409,

        UnprocessableEntity = 421,
        ValidationError = 422,
        PasswordPolicyViolation = 422,
        QuizHasEnrollments = 430,

        InternalServerError = 500,

        UserNotFound,
        InvalidToken,
        PasswordResetFailed,
        EmailSendFailed,
        EmailAlreadyExists,
        AccountNotVerified
    }
}
