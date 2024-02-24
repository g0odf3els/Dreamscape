using MediatR;

namespace Dreamscape.Application.Collections.Commands.UpdateCollection
{
    public sealed record UpdateCollectionCommand(
        string UserId,
        string CollectionId,
        string Name,
        string Description,
        bool IsPrivate = false) 
        : IRequest<CollectionViewModel>;
}
