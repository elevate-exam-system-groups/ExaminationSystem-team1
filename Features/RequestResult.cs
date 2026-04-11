using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Features.Account
{
    public record RequestResult<TResult>(TResult data, bool IsSuccess, ErrorCode errorCode);
}
