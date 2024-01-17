using MediatR;

namespace Dreamscape.Application.Collections.Commands.AppendFileToCollection
{
    public sealed record AppendFileToCollectionCommand(string UserId, string CollectionId, string FileId)
        : IRequest<CollectionViewModel>;
}
