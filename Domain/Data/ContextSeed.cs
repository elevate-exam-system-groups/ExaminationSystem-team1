using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExaminationSystem.Domain.Data
{
    public static class ContextSeed
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        public static async Task<string> SeedUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            #region Seed roles

            string[] roles = { "Admin", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            #endregion

            #region Seed Admin  
            if (await userManager.FindByEmailAsync("SuperAdmin@gmail.com") is null)
            {
                var admin = new User
                {
                    FullName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "0123456789",
                    accountStatus = AccountStatus.Active,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, "Abc@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            #endregion

            #region Seed Student
            //if (await userManager.FindByEmailAsync("student@gmail.com") is null)
            //{
            //    var student = new User
            //    {
            //        Id = "a0000001-0000-0000-0000-000000000001",
            //        FullName = "Test Student",
            //        Email = "student@gmail.com",
            //        UserName = "student@gmail.com",
            //        PhoneNumber = "0123456789",
            //        accountStatus = AccountStatus.Active,
            //        EmailConfirmed = true
            //    };
            //    var result = await userManager.CreateAsync(student, "Abc@123");
            //    if (result.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(student, "Student");
            //    }
            //}
            string studentId = "";
            var existingStudent = await userManager.FindByEmailAsync("student@gmail.com");
            if (existingStudent is null)
            {
                var student = new User
                {
                    FullName = "Test Student",
                    Email = "student@gmail.com",
                    UserName = "student@gmail.com",
                    accountStatus = AccountStatus.Active,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(student, "Abc@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(student, "Student");
                    studentId = student.Id; // الـ Id الحقيقي بعد الـ create
                }
            }
            else
            {
                studentId = existingStudent.Id;
            }

            return studentId;
            #endregion
        }

        public static async Task SeedDataAsync(Context context, string studentId)
        {
            #region Diplomas

            if (!context.Diplomas.Any())
            {
                var data = File.ReadAllText("Domain/Data/DataSeed/Diplomas.json");
                var diplomas = JsonSerializer.Deserialize<List<Diploma>>(data, _jsonOptions);
                if (diplomas?.Count > 0)
                {
                    //foreach (var diploma in diplomas)
                    //{
                    //    await context.Diplomas.AddRangeAsync(diploma);
                    //}
                    await context.Diplomas.AddRangeAsync(diplomas);
                    await context.SaveChangesAsync();
                }
            }
            #endregion

            #region Quizzes
            if (!context.Quizzes.Any())
            {
                var data = File.ReadAllText("Domain/Data/DataSeed/Quizzes.json");
                var Quizzes = JsonSerializer.Deserialize<List<Quiz>>(data, _jsonOptions);
                if (Quizzes?.Count > 0)
                {
                    //foreach (var quiz in Quizzes)
                    //{
                    //    await context.Quizzes.AddRangeAsync(quiz);
                    //}
                    await context.Quizzes.AddRangeAsync(Quizzes);
                    await context.SaveChangesAsync();
                }
            }
            #endregion

            #region Questions
            if (!context.Questions.Any())
            {
                var data = File.ReadAllText("Domain/Data/DataSeed/Questions.json");
                var Questions = JsonSerializer.Deserialize<List<Question>>(data, _jsonOptions);
                if (Questions?.Count > 0)
                {
                    //foreach (var question in Questions)
                    //{
                    //    await context.Questions.AddRangeAsync(question);
                    //}
                    await context.Questions.AddRangeAsync(Questions);
                    await context.SaveChangesAsync();
                }
            }
            #endregion

            #region Options
            if (!context.Options.Any())
            {
                var data = File.ReadAllText("Domain/Data/DataSeed/Options.json");
                var Options = JsonSerializer.Deserialize<List<Option>>(data, _jsonOptions);
                if (Options?.Count > 0)
                {
                    //foreach (var option in Options)
                    //{
                    //    await context.Options.AddRangeAsync(option);
                    //}
                    await context.Options.AddRangeAsync(Options);
                    await context.SaveChangesAsync();
                }
            }
            #endregion

            #region Enrollments

            if (!context.Enrollments.Any())
            {
                var enrollments = new List<Enrollment>
        {
            new Enrollment
            {
                Id = Guid.Parse("e1000000-0000-0000-0000-000000000001"),
                StudentId = studentId,
                DiplomaId = Guid.Parse("d1000000-0000-0000-0000-000000000001"),
                EnrollmentDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            }
        };
                await context.Enrollments.AddRangeAsync(enrollments);
                await context.SaveChangesAsync();
            }
            #endregion

        }
    }
}
