using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dreamscape.Application.Files.Commands.CreateFile
{
    public sealed record CreateFileCommand(IFormFile File, string UserId, string[]? Tags = null)
        : IRequest<ImageFileViewModel>;
}
