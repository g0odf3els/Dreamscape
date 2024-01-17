using MediatR;

namespace Dreamscape.Application.Collections.Commands.DeleteCollection
{
    public sealed record DeleteCollectionCommand(string UserId, string CollectionId)
        : IRequest<Unit>;
}
