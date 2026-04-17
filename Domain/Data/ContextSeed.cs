using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;

namespace ExaminationSystem.Domain.Data
{
    public static class ContextSeed
    {
        public static async Task SeedUserAsync(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new User()
                {
                    FullName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789",
                    accountStatus = AccountStatus.Active,
                    //Role = Role.Admin,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(User, "Abc@123");
            }

        }
    }
}
