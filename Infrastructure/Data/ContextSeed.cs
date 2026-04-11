using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Infrastructure.Data
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
                    PhoneNumber = "0123456789"
                };

                await userManager.CreateAsync(User, "Abc@123");
            }

        }
    }
}
