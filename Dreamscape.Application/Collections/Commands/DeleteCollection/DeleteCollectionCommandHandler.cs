using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Collections.Commands.DeleteCollection
{
    public class DeleteCollectionCommandHandler(
        IUserRepository userRepository,
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteCollectionCommand, Unit>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICollectionRepository _collectionRepository = collectionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeleteCollectionCommand request, CancellationToken cancellationToken)
        {
            var collection = await _collectionRepository.GetAsync(
               [c => c.Id.ToString() == request.CollectionId],
               null,
               cancellationToken
           ) ?? throw new NotFoundException(nameof(Collection), request.CollectionId);

            if (collection.OwnerId != request.UserId)
            {
                throw new ForbiddenException();
            }

            _collectionRepository.Delete(collection);
            await _unitOfWork.Save(cancellationToken);

            return new Unit();
        }
    }
}
