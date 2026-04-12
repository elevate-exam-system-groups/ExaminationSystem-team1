using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Account.AccountControllers.ViewModels.PasswordViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
