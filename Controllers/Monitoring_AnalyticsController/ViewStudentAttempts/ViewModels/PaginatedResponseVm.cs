namespace ExaminationSystem.Controllers.Monitoring_Analytics.ViewStudentAttempts.ViewModels
{
    public record PaginatedResponseVm<T>(
      List<T> Items,
      int TotalCount,
      int Page,
      int PageSize,
      int TotalPages,
      bool HasPreviousPage,
      bool HasNextPage
  );
}
