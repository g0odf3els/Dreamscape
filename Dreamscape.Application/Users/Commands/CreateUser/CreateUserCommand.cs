using MediatR;

namespace Dreamscape.Application.Users.Commands.CreateUser
{
    public sealed record CreateUserCommand(string Email, string Username, string Password)
        : IRequest<CreateUserView>;
}
