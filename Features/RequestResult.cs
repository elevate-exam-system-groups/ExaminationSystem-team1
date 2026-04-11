using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Features.Account
{
    public record RequestResult<TResult>(TResult data, bool IsSuccess, global::ExaminationSystem.Domain.Enums.ErrorCode errorCode);
}
