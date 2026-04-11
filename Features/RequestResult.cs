using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Features
{
    public record RequestResult<TResult>(TResult data, bool IsSuccess, ErrorCode errorCode);
}
