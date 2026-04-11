namespace ExaminationSystem.Features.Common
{
    public record RequestResult<TResult>
    {
        public TResult? Data { get; init; }
        public bool IsSuccess { get; init; }
        public ErrorCode ErrorCode { get; init; }


        public static RequestResult<TResult> Success(TResult data) =>
            new RequestResult<TResult>
            {
                Data = data,
                IsSuccess = true,
                ErrorCode = ErrorCode.None,

            };

        public static RequestResult<TResult> Failure(ErrorCode errorCode) =>
            new RequestResult<TResult>
            {
                Data = default,
                IsSuccess = false,
                ErrorCode = errorCode,

            };


    }
}
