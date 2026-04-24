namespace ExaminationSystem.Controllers.Shared.Enums
{
    public enum ResponseVmErrorCode
    {
        None = 0,

        InvalidCredentials,
        InvalidToken = 400,
        PasswordResetFailed = 400,
        EmailSendFailed = 500,
        EmailAlreadyExists = 409,
        AccountNotVerified = 403,
        ValidationError = 422,
        Unauthorized = 401,

        NotFound = 404,
        Conflict = 409,
        Success = 201,
        InternalServerError = 500,
        UserNotFound = 501
    }
}
