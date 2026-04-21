using ExaminationSystem.Features.StudentDashboard.DTOs;
using ExaminationSystem.Features.StudentDashboard.ExaminationSystem.Features.StudentDashboard.Queries;
using ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizIds;
using ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizzesCount;
using ExaminationSystem.Features.StudentDashboard.Queries.GetDiplomaDetails;
using ExaminationSystem.Features.StudentDashboard.Queries.GetEnrolledDiplomaIds;
using ExaminationSystem.Features.StudentDashboard.Queries.GetTotalQuizzesCount;

namespace ExaminationSystem.Features.StudentDashboard.Queries.GetEnrolledDiplomas
{
    public class GetEnrolledDiplomasQueryHandler
          : IRequestHandler<GetEnrolledDiplomasQuery, RequestResult<EnrolledDiplomaIdsDto>>
    {

        private readonly IMediator _mediator;
        public GetEnrolledDiplomasQueryHandler(IMediator mediator)
            => _mediator = mediator;

        public async Task<RequestResult<EnrolledDiplomaIdsDto>> Handle(
            GetEnrolledDiplomasQuery request, CancellationToken ct)
        {

            var idsResult = await _mediator.Send(
                new GetEnrolledDiplomaIdsQuery(request.StudentId), ct);

            var diplomaIds = idsResult.Data!.DiplomaIds;

            if (!diplomaIds.Any())
                return RequestResult<EnrolledDiplomaIdsDto>.Success(new EnrolledDiplomaIdsDto(new()));

            var diplomasResult = await _mediator.Send(
                new GetDiplomaDetailsQuery(diplomaIds), ct);
            var diplomas = diplomasResult.Data.Data!;

            var totalQuizzesResult = await _mediator.Send(
                new GetTotalQuizzesCountQuery(diplomaIds), ct);
            var totalQuizzes = totalQuizzesResult.Data.CountByDiplomaId!;

            var completedIdsResult = await _mediator.Send(
                new GetCompletedQuizIdsQuery(request.StudentId, diplomaIds), ct);
            var completedQuizIds = completedIdsResult.Data.QuizIds!;

            var completedQuizzesResult = await _mediator.Send(
                new GetCompletedQuizzesCountQuery(completedQuizIds), ct);
            var completedQuizzes = completedQuizzesResult.Data.CountByDiplomaId!;

            var result = diplomaIds
                .Where(id => diplomas.ContainsKey(id))
                .Select(id => new EnrolledDiplomaDto(
                    id,
                    diplomas[id].Title,
                    diplomas[id].Description,
                    totalQuizzes.GetValueOrDefault(id),
                    completedQuizzes.GetValueOrDefault(id),
                    CalculateProgress(
                        totalQuizzes.GetValueOrDefault(id),
                        completedQuizzes.GetValueOrDefault(id))
                ))
                .ToList();

            return RequestResult<EnrolledDiplomaIdsDto>.Success(new EnrolledDiplomaIdsDto(result));
        }

        private decimal CalculateProgress(int total, int completed)
            => total > 0 ? Math.Round((decimal)completed / total * 100, 1) : 0;
    }
}

