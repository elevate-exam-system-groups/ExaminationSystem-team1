using ExaminationSystem.Features.Common.Helpers;
using ExaminationSystem.Features.Common.Request;
using ExaminationSystem.Features.StudentDashboard.DTOs.Diploma;
using ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizIds;
using ExaminationSystem.Features.StudentDashboard.Queries.GetCompletedQuizzesCount;
using ExaminationSystem.Features.StudentDashboard.Queries.GetDiplomaDetails;
using ExaminationSystem.Features.StudentDashboard.Queries.GetEnrolledDiplomaIds;
using ExaminationSystem.Features.StudentDashboard.Queries.GetTotalQuizzesCount;

namespace ExaminationSystem.Features.StudentDashboard.Orchestrator.EnrolledDiplomas
{
    public class GetEnrolledDiplomasOrchestratorHandler
          : IRequestHandler<GetEnrolledDiplomasOrchestrator, RequestResult<EnrolledDiplomasListDto>>
    {

        private readonly IMediator _mediator;
        public GetEnrolledDiplomasOrchestratorHandler(IMediator mediator)
            => _mediator = mediator;

        public async Task<RequestResult<EnrolledDiplomasListDto>> Handle(
            GetEnrolledDiplomasOrchestrator request, CancellationToken ct)
        {

            var idsResult = await _mediator.Send(
                new GetEnrolledDiplomaIdsQuery(request.StudentId), ct);

            if (!idsResult.IsSuccess || !idsResult.Data.DiplomaIds.Any())  //=======================
                return RequestResult<EnrolledDiplomasListDto>.Success(
                    new EnrolledDiplomasListDto(new()));

            var diplomaIds = idsResult.Data.DiplomaIds;

            var diplomasResult = await _mediator.Send(
                new GetDiplomaDetailsQuery(diplomaIds), ct);
            var diplomas = diplomasResult.Data.Data;

            var totalQuizzesResult = await _mediator.Send(
                new GetTotalQuizzesCountQuery(diplomaIds), ct);
            var totalQuizzes = totalQuizzesResult.Data.CountByDiplomaId;

            var completedIdsResult = await _mediator.Send(
                new GetCompletedQuizIdsQuery(request.StudentId, diplomaIds), ct);
            var completedQuizIds = completedIdsResult.Data.QuizIds;

            var completedQuizzesResult = await _mediator.Send(
                new GetCompletedQuizzesCountQuery(completedQuizIds), ct);
            var completedQuizzes = completedQuizzesResult.Data.CountByDiplomaId;

            var result = diplomaIds
                .Where(id => diplomas.ContainsKey(id))
                .Select(id =>
                {
                    var total = totalQuizzes.GetValueOrDefault(id);
                    var completed = completedQuizzes.GetValueOrDefault(id);

                    return new EnrolledDiplomaDto(
                        id,
                        diplomas[id].Title,
                        diplomas[id].Description,
                        total,
                        completed,
                        StatisticsHelper.CalculatePercentage(total, completed));
                }).ToList();

            return RequestResult<EnrolledDiplomasListDto>.Success(
                new EnrolledDiplomasListDto(result));
        }

    }
}

