using Dreamscape.Application.Files.Queries;
using MediatR;

namespace Dreamscape.Application.Collections.Queries.GetPagedCollections
{
    public class GetPagedCollectionsQuery : IRequest<PagedList<CollectionViewModel>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 16;

        public string? OwnerId { get; set; }
    }
}
