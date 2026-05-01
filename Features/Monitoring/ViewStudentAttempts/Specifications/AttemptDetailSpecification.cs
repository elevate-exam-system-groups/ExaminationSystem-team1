using ExaminationSystem.Features.Common.Specifications;

namespace ExaminationSystem.Features.AdminManagement.ViewStudentAttempts.Specifications
{
    public class AttemptDetailSpecification : BaseSpecification<QuizAttempt>
    {
        public AttemptDetailSpecification(Guid attemptId)
        {

            var builder = new SpecificationBuilder<QuizAttempt>();

            builder.Add(a => a.Id == attemptId);

            Criteria = builder.Build()!;
        }
    }
}
