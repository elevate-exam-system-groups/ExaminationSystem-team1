using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ExaminationSystem.Infrastructure.Data
{
    public class Context : IdentityDbContext<User>//DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        //public DbSet<User> Users { get; set; }
        public DbSet<Diploma> Diplomas { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<AttemptAnswer> AttemptAnswers { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
            // Prevent multiple cascade paths error for AttemptAnswer
            modelBuilder.Entity<AttemptAnswer>()
                .HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AttemptAnswer>()
                .HasOne(a => a.Option)
                .WithMany()
                .HasForeignKey(a => a.OptionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
