using ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts.Specifications;

namespace ExaminationSystem.Features.MonitoringAndAnalytics.ViewStudentAttempts
{
    public class AttemptFilterSpecification : BaseSpecification<QuizAttempt>
    {
        public AttemptFilterSpecification(
            Guid? quizId = null,
            string? studentId = null,
            string? status = null,
            string? sortBy = "submitted_at",
            string? order = "desc"
        )
        {

            var builder = new SpecificationBuilder<QuizAttempt>();

            builder.Add(a => !a.IsDeleted);

            if (quizId.HasValue)
                builder.Add(a => a.QuizId == quizId.Value);

            if (!string.IsNullOrEmpty(studentId))
                builder.Add(a => a.StudentId == studentId);

            if (!string.IsNullOrEmpty(status)
                && Enum.TryParse<QuizAttemptStatus>(status, out var parsedStatus))
            {
                builder.Add(a => a.Status == parsedStatus);
            }

            Criteria = builder.Build()!;



            var isDescending = order?.ToLower() != "asc";

            switch (sortBy?.ToLower())
            {
                case "score":
                    ApplyOrderBy(a => a.Score ?? 0, isDescending);
                    break;
                case "student_name":
                    ApplyOrderBy(a => a.Student.FullName, isDescending);
                    break;
                case "quiz_title":
                    ApplyOrderBy(a => a.Quiz.Title, isDescending);
                    break;
                case "submitted_at":
                default:
                    ApplyOrderBy(a => a.SubmittedAt ?? a.StartTime, isDescending);
                    break;
            }
        }
    }
}
