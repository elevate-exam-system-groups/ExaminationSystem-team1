using System.Runtime.Serialization;

namespace ExaminationSystem.Domain.Models.Enums
{
    public enum QuizStatus
    {
        [EnumMember(Value = "Draft")]
        Draft,
        [EnumMember(Value = "Published")]
        Published
    }
}
