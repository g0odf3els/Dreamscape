using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Files.Queries.IsInUserCollection
{
    public class IsInUserCollectionQueryHandler(
        IUserRepository userRepository,
        IFileRepository fileRepository)
        : IRequestHandler<IsInUserCollectionQuery, bool>
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IFileRepository _fileRepository = fileRepository;

        public async Task<bool> Handle(IsInUserCollectionQuery request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetAsync(
                 [u => u.Id.ToString() == request.FileId],
                 [u => u.Collections],
                 cancellationToken
            ) ?? throw new NotFoundException(nameof(ImageFile), request.FileId);

            return file.Collections.Any(c => c.OwnerId == request.UserId);
        }
    }
}
