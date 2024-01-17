using MediatR;

namespace Dreamscape.Application.Files.Commands.AddTagToFile
{
    public sealed record AddTagToFileCommand(string UserId, string FileId, string Tag)
        : IRequest<ImageFileViewModel>;
}
