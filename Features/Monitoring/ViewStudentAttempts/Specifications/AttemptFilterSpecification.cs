using System.Globalization;
using ExaminationSystem.Features.Common.Specifications;

namespace ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Specifications
{
    public class AttemptFilterSpecification : BaseSpecification<QuizAttempt>
    {
        public AttemptFilterSpecification(
            Guid? quizId,
            string? studentId,
            string? status,
            AttemptSortField sortBy,
            OrderDirection orderDirection
        )
        {
            var builder = new SpecificationBuilder<QuizAttempt>();

            builder.Add(a => !a.isDeleted);

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

            var isDescending = orderDirection == OrderDirection.Desc;

            ApplyOrderBy(sortBy switch
            {
                AttemptSortField.Score => a => a.Score!,
                AttemptSortField.StartTime => a => a.StartTime,
                AttemptSortField.SubmittedAt => a => a.SubmittedAt!,
                _ => a => a.SubmittedAt!
            }, isDescending);
        }
    }
}
