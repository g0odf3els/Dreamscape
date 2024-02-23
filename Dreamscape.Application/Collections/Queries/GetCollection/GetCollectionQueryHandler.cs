using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Collections.Queries.GetCollection
{
    public class GetCollectionQueryHandler(
        ICollectionRepository collectionRepository,
        IMapper mapper)
        : IRequestHandler<GetCollectionQuery, CollectionViewModel>
    {
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IMapper _mapper = mapper;

        public async Task<CollectionViewModel> Handle(GetCollectionQuery request, CancellationToken cancellationToken)
        {
            var collection = await _collectionRepository.GetAsync(
              [c => c.Id.ToString() == request.CollectionId],
              [
                  c => c.Owner,
                  c => c.Files,
                  c => c.Tags
              ],
              cancellationToken
            ) ?? throw new NotFoundException(nameof(Collection), request.CollectionId);

            return _mapper.Map<CollectionViewModel>(collection);
        }
    }
}
