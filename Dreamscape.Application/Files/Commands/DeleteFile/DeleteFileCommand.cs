using MediatR;

namespace Dreamscape.Application.Files.Commands.DeleteFile
{
    public sealed record DeleteFileCommand(string UserId, string FileId)
        : IRequest<bool>;
}
