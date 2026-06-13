namespace ExaminationSystem.Features.Authentication.Queries
{
    public record LoginQueryResponse(bool IsSuccess, string UserId, string Username, string Email, string Role);
}
