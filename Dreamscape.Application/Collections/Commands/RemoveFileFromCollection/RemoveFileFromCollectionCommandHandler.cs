using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Collections.Commands.RemoveFileFromCollection
{
    public class RemoveFileFromCollectionCommandHandler(
        IUserRepository userRepository,
        IFileRepository fileRepository,
        ICollectionRepository collectionRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<RemoveFileFromCollectionCommand, Unit>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IFileRepository _fileRepository = fileRepository;
        readonly ICollectionRepository _collectionRepository = collectionRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(RemoveFileFromCollectionCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
             [u => u.Id == request.UserId],
             [u => u.Collections],
             cancellationToken
           ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var file = await _fileRepository.GetAsync(
              [c => c.Id.ToString() == request.FileId],
              [u => u.Collections],
              cancellationToken
            ) ?? throw new NotFoundException(nameof(ImageFile), request.FileId);

            foreach (var collection in file.Collections.ToList().Where(u => u.OwnerId.ToString() == user.Id))
            {
                var fileToRemove = collection.Files.FirstOrDefault(f => f.Id == file.Id);
                if (fileToRemove != null)
                {
                    collection.Files.Remove(fileToRemove);
                    await _unitOfWork.Save(cancellationToken);
                }
            }

            await _unitOfWork.Save(cancellationToken);
            return new Unit();
        }
    }
}
