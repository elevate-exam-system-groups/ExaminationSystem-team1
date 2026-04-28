using ExaminationSystem.Features.Common.Paginated.DTOs;

namespace ExaminationSystem.Controllers.AdminManagementControllers.ViewStudentAttempts.Pagination
{
    public static class PaginationMapper
    {
        public static PaginatedResponseVm<TViewModel> ToViewModel<TDto, TViewModel>(
            this PaginatedResponseDto<TDto> dto, Func<TDto, TViewModel> mapItem)
        {
            return new PaginatedResponseVm<TViewModel>(
                Items: dto.Items.Select(mapItem)
                                .ToList(),
                TotalCount: dto.TotalCount,
                Page: dto.Page,
                PageSize: dto.PageSize,
                TotalPages: dto.TotalPages,
                HasPreviousPage: dto.HasPreviousPage,
                HasNextPage: dto.HasNextPage
            );
        }
    }
}
