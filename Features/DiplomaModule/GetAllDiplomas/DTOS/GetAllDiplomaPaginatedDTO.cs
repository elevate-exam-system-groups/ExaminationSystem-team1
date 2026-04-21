namespace ExaminationSystem.Features.DiplomaModule.GetAllDiplomas.DTOS
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
