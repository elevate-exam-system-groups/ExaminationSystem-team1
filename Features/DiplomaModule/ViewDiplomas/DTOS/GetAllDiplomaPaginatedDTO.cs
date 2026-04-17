namespace ExaminationSystem.Features.DiplomaModule.ViewDiplomas.DTOS
{
    public record GetAllDiplomaPaginatedDTO
     (
     List<GetPublishedDiplomaResponseDTO> Data,
     int Page,
     int PerPage,
     int Total,
     int TotalPages
     );
}
