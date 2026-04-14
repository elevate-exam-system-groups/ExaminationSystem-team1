using ExaminationSystem.Controllers.Shared.Enums;

namespace ExaminationSystem.Controllers.Shared
{
    public class ResponseViewModel<T> where T : class
    {
        public bool isSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public ResponseVmErrorCode? errorCode { get; set; }
        public static ResponseViewModel<T> Success(T data, string message = "Success")
        {
            return new ResponseViewModel<T>
            {
                isSuccess = true,
                Data = data,
                Message = message,
                errorCode = ResponseVmErrorCode.None
            };
        }
        public static ResponseViewModel<T> Failure(string message = "Request Failed", ResponseVmErrorCode? ErrorCode = null)
        {
            return new ResponseViewModel<T>
            {
                isSuccess = false,
                Data = null,
                Message = message,
                errorCode = ErrorCode
            };
        }

    }
}
