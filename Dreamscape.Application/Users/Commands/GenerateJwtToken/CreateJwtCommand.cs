using MediatR;

namespace Dreamscape.Application.Users.Commands.GenerateJwtToken
{
    public record CreateJwtCommand(string Username, string Password)
        : IRequest<CreateJwtCommandView>;
}
