using MediatR;

namespace Dreamscape.Application.Files.Commands.RemoveTagFromFile
{
    public sealed record RemoveTagFromFileCommand(string UserId, string FileId, string Tag)
        : IRequest<ImageFileViewModel>;
}
