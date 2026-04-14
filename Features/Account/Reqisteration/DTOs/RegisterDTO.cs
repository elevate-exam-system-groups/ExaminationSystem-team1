using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.AuthModule.Account.DTOs
{
    public record RegisterDTO([EmailAddress]string Email,string FullName, string Password);
}
