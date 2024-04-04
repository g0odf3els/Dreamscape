using MediatR;

namespace Dreamscape.Application.Collections.Commands.RemoveFileFromCollection
{
    public sealed record RemoveFileFromCollectionCommand(string UserId, string CollectionId, string FileId)
        : IRequest<CollectionViewModel>;
}
