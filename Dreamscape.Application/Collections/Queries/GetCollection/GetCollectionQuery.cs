using MediatR;

namespace Dreamscape.Application.Collections.Queries.GetCollection
{
    public sealed record GetCollectionQuery(string CollectionId)
        : IRequest<CollectionViewModel>;
}
