namespace ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.Filter
{
    public record AttemptFilterRequest
    {

        [FromQuery(Name = "quiz_id")]
        public Guid? QuizId { get; init; }


        [FromQuery(Name = "student_id")]
        public string? StudentId { get; init; }


        public string? Status { get; init; }


        [FromQuery(Name = "sort_by")]
        public AttemptSortField SortBy { get; init; } = AttemptSortField.SubmittedAt;


        public OrderDirection Order { get; init; } = OrderDirection.Desc;


        public int Page { get; init; } = 1;


        [FromQuery(Name = "page_size")]
        public int PageSize { get; init; } = 20;

    }
}
