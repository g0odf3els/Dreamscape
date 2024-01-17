using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Files.Commands.AddTagToFile
{
    public class AddTagToFileCommandHandler(
        IFileRepository fileRepository,
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<AddTagToFileCommand, ImageFileViewModel>
    {
        private readonly IFileRepository _fileRepository = fileRepository;
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ImageFileViewModel> Handle(AddTagToFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetAsync(
                [
                    f => f.Id.ToString() == request.FileId
                ],
                [
                    f => f.Uploader,
                    f => f.Tags
                ],
                cancellationToken
            ) ?? throw new NotFoundException(nameof(ImageFile), request.FileId);

            if (file.UploaderId != request.UserId)
            {
                throw new ForbiddenException();
            }

            var attachedTag = await _tagRepository.GetAsync([t => t.Name == request.Tag], [t => t.Files], cancellationToken);

            if (attachedTag == null)
            {
                attachedTag = _tagRepository.Create(new Tag { Name = request.Tag });
            }
            else if (file.Tags.Contains(attachedTag))
            {
                throw new AlreadyExistException();
            }

            file.Tags.Add(attachedTag);

            _fileRepository.Update(file);

            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<ImageFileViewModel>(file);
        }
    }
}
