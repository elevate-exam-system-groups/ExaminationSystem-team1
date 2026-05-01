using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;

namespace ExaminationSystem.Domain.Data
{
    public static class ContextSeed
    {
        public static async Task SeedAsync(Context context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUserAsync(userManager);
            await SeedDataAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Role.Student.ToString()));
            }
        }

        private static async Task SeedUserAsync(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                // Seed Admin
                var admin = new User()
                {
                    FullName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789",
                    accountStatus = AccountStatus.Active,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "P@ssw0rd123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Role.Admin.ToString());
                }

                // Seed Student
                var student = new User()
                {
                    FullName = "Sample Student",
                    Email = "student@gmail.com",
                    UserName = "student",
                    PhoneNumber = "01122334455",
                    accountStatus = AccountStatus.Active,
                    EmailConfirmed = true
                };

                result = await userManager.CreateAsync(student, "P@ssw0rd123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(student, Role.Student.ToString());
                }
            }
        }

        private static async Task SeedDataAsync(Context context)
        {
            if (!context.Diplomas.Any())
            {
                var diplomas = new List<Diploma>
                {
                    new Diploma
                    {
                        Title = ".NET Web Development",
                        Description = "Comprehensive track for building modern web apps with ASP.NET Core.",
                        Status = DiplomaStatus.Published,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    },
                    new Diploma
                    {
                        Title = "Frontend Mastery with Angular",
                        Description = "Master modern frontend development using Angular.",
                        Status = DiplomaStatus.Published,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System"
                    }
                };

                context.Diplomas.AddRange(diplomas);
                await context.SaveChangesAsync();

                // Seed Quizzes for the first diploma
                var quiz = new Quiz
                {
                    DiplomaId = diplomas[0].Id,
                    Title = "C# Fundamentals",
                    Instructions = "Answer all questions. Passing score is 60%.",
                    PassScore = 60,
                    DurationInMinutes = 30,
                    Status = QuizStatus.Published,
                    PublishedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                context.Quizzes.Add(quiz);
                await context.SaveChangesAsync();

                // Seed Questions and Options
                var question1 = new Question
                {
                    QuizId = quiz.Id,
                    Text = "What is the base class for all types in C#?",
                    OrderIndex = 1,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                var question2 = new Question
                {
                    QuizId = quiz.Id,
                    Text = "Which keyword is used to define a constant in C#?",
                    OrderIndex = 2,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };

                context.Questions.AddRange(question1, question2);
                await context.SaveChangesAsync();

                var options = new List<QuestionOption>
                {
                    new QuestionOption { QuestionId = question1.Id, Text = "System.Object", IsCorrect = true, CreatedBy = "System" },
                    new QuestionOption { QuestionId = question1.Id, Text = "System.Base", IsCorrect = false, CreatedBy = "System" },
                    new QuestionOption { QuestionId = question1.Id, Text = "System.Root", IsCorrect = false, CreatedBy = "System" },
                    
                    new QuestionOption { QuestionId = question2.Id, Text = "const", IsCorrect = true, CreatedBy = "System" },
                    new QuestionOption { QuestionId = question2.Id, Text = "static", IsCorrect = false, CreatedBy = "System" },
                    new QuestionOption { QuestionId = question2.Id, Text = "readonly", IsCorrect = false, CreatedBy = "System" }
                };

                context.Options.AddRange(options);
                await context.SaveChangesAsync();
            }
        }
    }
}
