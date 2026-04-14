namespace ExaminationSystem.Controllers.Shared.Enums
{
    public enum ResponseVmErrorCode
    {
        None = 0,
        InvalidCredentials,
        UserNotFound = 404,
        InvalidToken = 400,
        PasswordResetFailed = 400,
        EmailSendFailed = 500,
        EmailAlreadyExists = 409,
        AccountNotVerified = 403,
        ValidationError = 422,
        //InvalidInput = 1,
        //NotFound = 2,
        Unauthorized = 401,
        //Forbidden = 4,
        InternalServerError = 500,
        //BadRequest = 6,
        //Conflict = 7,
        //ServiceUnavailable = 8

    }
}
