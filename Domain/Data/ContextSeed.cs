using ExaminationSystem.Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;
using ExaminationSystem.Domain.Models;
using System.Text.Json.Serialization;
using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;

namespace ExaminationSystem.Domain.Data
{
    public static class ContextSeed
    {
        public static async Task SeedAsync(Context context)
        {
            await SeedUserAsync(context);
            await SeedDataAsync(context);
        }

        private static async Task SeedUserAsync(Context context)
        {
            if (!await context.Users.AnyAsync())
            {
                // Seed Admin
                var admin = new User()
                {
                    Id = "admin-id",
                    FullName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789",
                    accountStatus = AccountStatus.Active,
                    EmailConfirmed = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd123!")
                };

                // Seed Student
                var student = new User()
                {
                    Id = "a0000001-0000-0000-0000-000000000001",
                    FullName = "Sample Student",
                    Email = "student@gmail.com",
                    UserName = "student",
                    PhoneNumber = "01122334455",
                    accountStatus = AccountStatus.Active,
                    EmailConfirmed = true,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd123!")
                };

                context.Users.AddRange(admin, student);
                await context.SaveChangesAsync();
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

                var options = new List<Option>
                {
                    new Option { QuestionId = question1.Id, Text = "System.Object", IsCorrect = true, CreatedBy = "System" },
                    new Option { QuestionId = question1.Id, Text = "System.Base", IsCorrect = false, CreatedBy = "System" },
                    new Option { QuestionId = question1.Id, Text = "System.Root", IsCorrect = false, CreatedBy = "System" },
                    
                    new Option { QuestionId = question2.Id, Text = "const", IsCorrect = true, CreatedBy = "System" },
                    new Option { QuestionId = question2.Id, Text = "static", IsCorrect = false, CreatedBy = "System" },
                    new Option { QuestionId = question2.Id, Text = "readonly", IsCorrect = false, CreatedBy = "System" }
                };

                context.Options.AddRange(options);
                await context.SaveChangesAsync();
            }
        }
    }
}
