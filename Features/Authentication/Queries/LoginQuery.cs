using MediatR;

namespace ExaminationSystem.Features.Authentication.Queries
{
    public record LoginQuery(string UsernameOrEmail, string Password) : IRequest<LoginQueryResponse>;
}
