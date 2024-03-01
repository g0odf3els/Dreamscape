using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dreamscape.Application.Users.Commands.UpdateUserProfileImage
{
    public sealed record UpdateUserProfileImageCommand(IFormFile File, string UserId) 
        : IRequest<UserViewModel>;
}
