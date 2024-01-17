using MediatR;

namespace Dreamscape.Application.Collections.Commands.CreateCollection
{
    public record CreateCollectionCommand(string UserId, string CollectionName)
        : IRequest<CollectionViewModel>;
}
