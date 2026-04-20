using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Models.Enums;
using ExaminationSystem.Domain.Models;
using ExaminationSystem.Features.StudentDashboard.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.StudentDashboard.Test
{
    public class GetStudentDashboardQueryHandler
        : IRequestHandler<GetStudentDashboardQuery, RequestResult<StudentDashboardResponse>>
    {
        private readonly IGeneralRepository<Enrollment> _enrollmentRepo;
        private readonly IGeneralRepository<QuizAttempt> _attemptRepo;
        private readonly IGeneralRepository<Quiz> _quizRepo;
        private readonly IGeneralRepository<Diploma> _diplomaRepo;
        private readonly IMemoryCache _cache;

        public GetStudentDashboardQueryHandler(
            IGeneralRepository<Enrollment> enrollmentRepo,
            IGeneralRepository<QuizAttempt> attemptRepo,
            IGeneralRepository<Quiz> quizRepo,
            IMemoryCache cache)
        {
            _enrollmentRepo = enrollmentRepo;
            _attemptRepo = attemptRepo;
            _quizRepo = quizRepo;
            _cache = cache;
        }

        public async Task<RequestResult<StudentDashboardResponse>> Handle(
            GetStudentDashboardQuery request, CancellationToken ct)
        {
            var cacheKey = $"dashboard_student_{request.StudentId}";

            // Try cache first
            if (_cache.TryGetValue(cacheKey, out StudentDashboardResponse? cached))
                return RequestResult<StudentDashboardResponse>.Success(cached!);

            // 1️⃣ Get Enrolled Diplomas with Progress
            var enrolledDiplomas = await GetEnrolledDiplomasAsync(request.StudentId, ct);

            // 2️⃣ Get Recent Attempts (last 5)
            var recentAttempts = await GetRecentAttemptsAsync(request.StudentId, ct);

            // 3️⃣ Get Overall Stats
            var overallStats = await GetOverallStatsAsync(request.StudentId, ct);

            var response = new StudentDashboardResponse(
                enrolledDiplomas,
                recentAttempts,
                overallStats
            );

            // Cache for 60 seconds
            _cache.Set(cacheKey, response, TimeSpan.FromSeconds(60));

            return RequestResult<StudentDashboardResponse>.Success(response);
        }

        private async Task<List<EnrolledDiplomaDto>> GetEnrolledDiplomasAsync(
        string studentId, CancellationToken ct)
        {
            // 1️⃣ Get enrolled diploma IDs ONLY
            var enrolledDiplomaIds = await _enrollmentRepo
                .Get(e => e.StudentId == studentId && !e.isDeleted)
                .Select(e => e.DiplomaId)
                .ToListAsync(ct);

            if (!enrolledDiplomaIds.Any())
                return new List<EnrolledDiplomaDto>();


            // 2️⃣ Get diploma details
            var diplomas = await _diplomaRepo
                .Get(d => enrolledDiplomaIds.Contains(d.Id)
                       && d.Status == DiplomaStatus.Published
                       && !d.isDeleted)
                .Select(d => new
                {
                    d.Id,
                    d.Title,
                    d.Description
                }).ToDictionaryAsync(k => k.Id, ct);

            // 3️⃣ Get total quizzes per diploma
            var totalQuizzesByDiploma = await _quizRepo
                .Get(q => enrolledDiplomaIds.Contains(q.DiplomaId)
                       && q.Status == QuizStatus.Published
                       && !q.isDeleted)
                .GroupBy(q => q.DiplomaId)
                .Select(g => new { DiplomaId = g.Key, TotalCount = g.Count() })
                .ToDictionaryAsync(k => k.DiplomaId, v => v.TotalCount, ct);

            // 4️⃣ Get completed quizzes per diploma
            var completedByDiploma = await _attemptRepo
                .Get(a => a.StudentId == studentId
                       && enrolledDiplomaIds.Contains(a.Quiz.DiplomaId)
                       && a.Status != QuizAttemptStatus.InProgress)
                .Select(a => new { a.Quiz.DiplomaId, a.QuizId })
                .Distinct()
                .GroupBy(x => x.DiplomaId)
                .Select(g => new { DiplomaId = g.Key, CompletedCount = g.Count() })
                .ToDictionaryAsync(k => k.DiplomaId, v => v.CompletedCount, ct);

            // 5️⃣ Build response
            var result = new List<EnrolledDiplomaDto>();

            foreach (var diplomaId in enrolledDiplomaIds)
            {
                if (!diplomas.TryGetValue(diplomaId, out var diploma))
                    continue;

                totalQuizzesByDiploma.TryGetValue(diplomaId, out var totalQuizzes);
                completedByDiploma.TryGetValue(diplomaId, out var completedQuizzes);

                var progressPercentage = totalQuizzes > 0
                    ? Math.Round((decimal)completedQuizzes / totalQuizzes * 100, 1)
                    : 0;

                result.Add(new EnrolledDiplomaDto(
                    diplomaId,
                    diploma.Title,
                    diploma.Description,
                    totalQuizzes,
                    completedQuizzes,
                    progressPercentage
                ));
            }

            return result;
        }

        private async Task<List<RecentAttemptDto>> GetRecentAttemptsAsync(
        string studentId, CancellationToken ct)
        {
            // ✓ Select مباشر - EF Core هيعمل الـ Join تلقائي
            var attempts = await _attemptRepo
                .Get(a => a.StudentId == studentId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(5)
                .Select(a => new RecentAttemptDto(
                    a.Id,
                    a.QuizId,
                    a.Quiz.Title,           // ✓ EF Core هيعمل Join مع Quiz
                    a.Quiz.Diploma.Title,   // ✓ EF Core هيعمل Join مع Diploma
                    a.Status.ToString(),
                    a.Score,
                    a.IsPassed,
                    a.SubmittedAt
                ))
                .ToListAsync(ct);

            return attempts;
        }

        private async Task<OverallStatsDto> GetOverallStatsAsync(
       string studentId, CancellationToken ct)
        {
            // ✓ جلب الحقول المطلوبة فقط
            var attempts = await _attemptRepo
                .Get(a => a.StudentId == studentId
                       && a.Status != QuizAttemptStatus.InProgress)
                .Select(a => new
                {
                    a.Score,
                    a.IsPassed
                })
                .ToListAsync(ct);

            if (!attempts.Any())
                return new OverallStatsDto(0, 0, 0);

            var totalQuizzesTaken = attempts.Count;

            var avgScore = attempts.Average(a => a.Score ?? 0);

            var passedCount = attempts.Count(a => a.IsPassed == true);

            var passRate = Math.Round((decimal)passedCount / totalQuizzesTaken * 100, 1);

            return new OverallStatsDto(
                totalQuizzesTaken,
                Math.Round(avgScore, 1),
                passRate
            );
        }


    }
}


