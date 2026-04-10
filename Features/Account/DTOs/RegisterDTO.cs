using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Account.DTOs
{
    public record RegisterDTO([EmailAddress]string Email,string FullName, string Password);
}
