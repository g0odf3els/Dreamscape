using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Files.Commands.DeleteFile
{
    public class DeleteFileCommandHandler(
        IFileRepository fileRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteFileCommand, bool>
    {
        private readonly IFileRepository _fileRepository = fileRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(
               [u => u.Id == request.UserId],
               [u => u.Collections],
               cancellationToken
            ) ?? throw new NotFoundException(nameof(User), request.UserId);

            var file = await _fileRepository.GetAsync(
               [f => f.Id.ToString() == request.FileId],
               [f => f.Uploader],
               cancellationToken
           ) ?? throw new NotFoundException(nameof(ImageFile), request.FileId);

            if (file.Uploader.Id != user.Id)
            {
                throw new ForbiddenException();
            }

            _fileRepository.Delete(file);
            await _unitOfWork.Save(cancellationToken);

            return true;
        }
    }
}
