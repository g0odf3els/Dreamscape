using MediatR;

namespace Dreamscape.Application.Collections.Commands.RemoveFileFromCollection
{
    public sealed record RemoveFileFromCollectionCommand(string UserId, string FileId)
        : IRequest<Unit>;
}
