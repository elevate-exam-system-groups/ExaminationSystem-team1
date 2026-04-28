namespace ExaminationSystem.Features.Common.Paginated
{
    public static class PaginationValidator
    {
        public static RequestResult<bool> ValidatePage(int page)
        {
            return page < 1
                ? RequestResult<bool>.Failure("Page must be greater than 0", RequestErrorCode.ValidationError)
                : RequestResult<bool>.Success(true);
        }

        public static RequestResult<bool> ValidatePageSize(int pageSize)
        {
            return pageSize < 1 || pageSize > 100
                ? RequestResult<bool>.Failure("Page size must be between 1 and 100", RequestErrorCode.ValidationError)
                : RequestResult<bool>.Success(true);
        }

        public static RequestResult<bool> ValidateStatus<TEnum>(string? status) where TEnum : struct, Enum
        {
            if (string.IsNullOrEmpty(status))
                return RequestResult<bool>.Success(true);

            return Enum.TryParse<TEnum>(status, out _)
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(
                    $"Status must be one of: {string.Join(", ", Enum.GetNames<TEnum>())}",
                    RequestErrorCode.ValidationError);
        }
    }
}
