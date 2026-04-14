using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Controllers.AccountController.ViewModels.LoginViewModels
{
    public class LoginRequestVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
