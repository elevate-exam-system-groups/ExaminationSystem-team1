using ExaminationSystem.Features.Common.Helpers;
using ExaminationSystem.Features.StudentDashboard.DTOs;

namespace ExaminationSystem.Features.StudentDashboard.Queries.EnrolledDiplomas
{
    namespace ExaminationSystem.Features.StudentDashboard.Queries.EnrolledDiplomas
    {
        public class GetEnrolledDiplomasQueryHandler
             : IRequestHandler<GetEnrolledDiplomasQuery, RequestResult<EnrolledDiplomasResponseDto>>
        {

            private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
            public GetEnrolledDiplomasQueryHandler(IGeneralRepository<Enrollment> enrollmentRepo)
            {
                _enrollmentRepo = enrollmentRepo;
            }

            public async Task<RequestResult<EnrolledDiplomasResponseDto>> Handle(
                GetEnrolledDiplomasQuery request, CancellationToken ct)
            {

                var rows = await _enrollmentRepo
                    .Get(e => e.StudentId == request.StudentId)
                    .AsSplitQuery()
                    .Select(e => new
                    {
                        e.DiplomaId,
                        e.Diploma.Title,
                        e.Diploma.Description,

                        Total = e.Diploma.Quizzes
                            .Count(q => q.Status == QuizStatus.Published
                                     && !q.IsDeleted),

                        Completed = e.Diploma.Quizzes
                            .Count(q => q.Status == QuizStatus.Published
                                     && !q.IsDeleted
                                     && q.QuizAttempts.Any(
                                            a => a.StudentId == request.StudentId
                                              && !a.IsDeleted
                                              && a.Status != QuizAttemptStatus.InProgress))

                    }).ToListAsync(ct);

                var result = rows
                    .Select(r => new EnrolledDiplomaDto(
                        r.DiplomaId,
                        r.Title,
                        r.Description,
                        r.Total,
                        r.Completed,
                        StatisticsHelper.CalculatePercentage(r.Total, r.Completed)

                    )).ToList();


                return RequestResult<EnrolledDiplomasResponseDto>.Success(
                    new EnrolledDiplomasResponseDto(result));
            }
        }
    }
}

