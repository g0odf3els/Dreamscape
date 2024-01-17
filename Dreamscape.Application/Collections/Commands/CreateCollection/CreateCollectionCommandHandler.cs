using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Collections.Commands.CreateCollection
{
    public class CreateCollectionCommandHandler(
        IUserRepository userRepository,
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<CreateCollectionCommand, CollectionViewModel>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;

        public async Task<CollectionViewModel> Handle(CreateCollectionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
                [u => u.Id == request.UserId],
                [u => u.Collections],
                cancellationToken
            ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var collection = new Collection()
            {
                Name = request.CollectionName,
                Owner = user,
                OwnerId = request.UserId,
            };

            _collectionRepository.Create(collection);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CollectionViewModel>(collection);
        }
    }
}
