using MediatR;

namespace Dreamscape.Application.Users.Queries.GetUser
{
    public sealed record GetUserQuery(string Id)
        : IRequest<UserViewModel>;
}
