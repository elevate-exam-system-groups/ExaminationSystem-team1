using ExaminationSystem.Features.Common.Specifications;

namespace ExaminationSystem.Features.AdminManagement.PerformanceAnalytics.Specifications
{
    public class AnalyticsFilterSpecification : BaseSpecification<QuizAttempt>
    {

        public AnalyticsFilterSpecification(DateTime? from = null, DateTime? to = null)
        {

            var builder = new SpecificationBuilder<QuizAttempt>();

            builder.Add(a => !a.isDeleted && a.Status != QuizAttemptStatus.InProgress);

            if (from.HasValue)
                builder.Add(a => a.SubmittedAt >= from.Value);

            if (to.HasValue)
                builder.Add(a => a.SubmittedAt <= to.Value);

            Criteria = builder.Build()!;
        }
    }
}
