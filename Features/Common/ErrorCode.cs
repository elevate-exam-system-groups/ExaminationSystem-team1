namespace ExaminationSystem.Features.Common
{
    public enum ErrorCode
    {
        None = 0,
        UserNotFound = 1001,
        InvalidToken = 1002,
        PasswordResetFailed = 1003,
        EmailSendFailed = 1004,
        EmailAlreadyExists = 1005,
        AccountNotVerified = 1006
    }
}
