
using System.Runtime.Serialization;

namespace ExaminationSystem.Domain.Models.Enums
{
    public enum Role
    {
        [EnumMember(Value = "Student")]
        Student,
        [EnumMember(Value = "Admin")]
        Admin
    }
}
