using AutoMapper;
using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Dreamscape.Application.Collections.Queries.GetPagedCollections
{
    public class GetPagedCollectionsQueryHandler(
        ICollectionRepository collectionRepository,
        IMapper mapper)
        : IRequestHandler<GetPagedCollectionsQuery, PagedList<CollectionViewModel>>
    {
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IMapper _mapper = mapper;

        public async Task<PagedList<CollectionViewModel>> Handle(GetPagedCollectionsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Collection, bool>>[]? filterExpressions = [];

            if (request.OwnerId != null)
            {
                filterExpressions = filterExpressions.Append(c => c.OwnerId == request.OwnerId).ToArray();
            }

            if (request.Search != null && request.Search.Length > 0) 
            {
                filterExpressions = filterExpressions.Append(c => c.Name == request.Search).ToArray();
            }

            var result = await _collectionRepository.GetPagedAsync(
              request.Page,
              request.PageSize,
              filterExpressions,
              null,
              [
                  c => c.Files,
                  c => c.Owner,
                  c => c.Tags
              ],
              false,
              cancellationToken
          );

            return _mapper.Map<PagedList<CollectionViewModel>>(result);
        }
    }
}
