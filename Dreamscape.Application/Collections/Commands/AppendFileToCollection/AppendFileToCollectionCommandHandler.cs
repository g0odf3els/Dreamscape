using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Collections.Commands.AppendFileToCollection
{
    public class AppendFileToCollectionCommandHandler(
        IUserRepository userRepository,
        IFileRepository fileRepository,
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<AppendFileToCollectionCommand, CollectionViewModel>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IFileRepository _fileRepository = fileRepository;
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;

        public async Task<CollectionViewModel> Handle(AppendFileToCollectionCommand request, CancellationToken cancellationToken)
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
                  c => c.Files
              ],
              cancellationToken
            ) ?? throw new NotFoundException(nameof(Collection), request.CollectionId);

            if (collection.OwnerId != request.UserId)
            {
                throw new ForbiddenException();
            }

            var file = await _fileRepository.GetAsync(
              [c => c.Id.ToString() == request.FileId],
              null,
              cancellationToken
            ) ?? throw new NotFoundException(nameof(ImageFile), request.FileId);

            collection.Files.Add(file);

            _collectionRepository.Update(collection);

            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<CollectionViewModel>(collection);
        }
    }
}
