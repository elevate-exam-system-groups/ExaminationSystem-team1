using System.Runtime.Serialization;

namespace ExaminationSystem.ExaminationSystem.Domain.Models.Enums
{
    public enum AccountStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Locked")]
        Locked
    }
}
