using System.Runtime.Serialization;

namespace ExaminationSystem.Domain.Models.Enums
{
    public enum QuizAttemptStatus
    {
        [EnumMember(Value = "InProgress")]
        InProgress,
        [EnumMember(Value = "Submitted")]
        Submitted,
        [EnumMember(Value = "TimedOut")]
        TimedOut
    }
}
