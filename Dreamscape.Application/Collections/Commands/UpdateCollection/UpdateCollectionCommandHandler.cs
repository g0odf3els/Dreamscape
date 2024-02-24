using MediatR;
using Dreamscape.Application.Repositories;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Domain.Entities;
using Accord.IO;
using AutoMapper;

namespace Dreamscape.Application.Collections.Commands.UpdateCollection
{
    public class UpdateCollectionCommandHandler(
        IUserRepository userRepository,

        ICollectionRepository collectionRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateCollectionCommand, CollectionViewModel>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IMapper _mapper = mapper;
        readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CollectionViewModel> Handle(UpdateCollectionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
                [u => u.Id == request.UserId],
                [u => u.Collections],
                cancellationToken
            ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var collection = await _collectionRepository.GetAsync(
             [c => c.Id.ToString() == request.CollectionId],
             [
                 c => c.Owner,
                 c => c.Files,
                 c => c.Tags
             ],
             cancellationToken
           ) ?? throw new NotFoundException(nameof(Collection), request.CollectionId);

            if (collection.OwnerId != request.UserId)
            {
                throw new ForbiddenException();
            }

            collection.Name = request.Name;
            collection.Description = request.Description;
            collection.IsPrivate = request.IsPrivate;

            _collectionRepository.Update(collection);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CollectionViewModel>(collection);
        }
    }
}
