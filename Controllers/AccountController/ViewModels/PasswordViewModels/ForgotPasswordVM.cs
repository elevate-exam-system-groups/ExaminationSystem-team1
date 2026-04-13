using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Controllers.AccountController.ViewModels.PasswordViewModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}
