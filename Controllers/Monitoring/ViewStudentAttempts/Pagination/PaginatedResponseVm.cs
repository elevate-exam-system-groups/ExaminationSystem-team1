namespace ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.Pagination
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
