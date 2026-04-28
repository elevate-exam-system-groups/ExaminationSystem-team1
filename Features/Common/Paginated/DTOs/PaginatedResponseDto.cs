namespace ExaminationSystem.Features.Common.Paginated.DTOs
{
    public record PaginatedResponseDto<T>(
        List<T> Items,
        int TotalCount,
        int Page,
        int PageSize,
        int TotalPages,
        bool HasPreviousPage,
        bool HasNextPage
    );
}
