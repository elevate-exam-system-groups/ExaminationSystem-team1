using ExaminationSystem.Features.Common.Specifications;

namespace ExaminationSystem.Features.StudentDashboard.Specifications
{
    public class StudentAttemptSpecification : BaseSpecification<QuizAttempt>
    {
        public StudentAttemptSpecification(string studentId, bool onlyCompleted = true)
        {

            var builder = new SpecificationBuilder<QuizAttempt>();

            builder.Add(a => a.StudentId == studentId);

            if (onlyCompleted)
                builder.Add(a => a.Status != QuizAttemptStatus.InProgress);

            Criteria = builder.Build()!;

            ApplyOrderBy(a => a.CreatedAt, isDescending: true);
        }
    }
}
