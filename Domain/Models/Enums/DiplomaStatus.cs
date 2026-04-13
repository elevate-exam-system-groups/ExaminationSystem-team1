using System.Runtime.Serialization;

namespace ExaminationSystem.Domain.Models.Enums
{
    public enum DiplomaStatus
    {
        [EnumMember(Value = "Published")]
        Published,
        [EnumMember(Value = "Draft")]
        Draft,
    }
}
