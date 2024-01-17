using AutoMapper;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Files.Commands.RemoveTagFromFile
{
    public class RemoveTagFromFileCommandHandler(
        IFileRepository fileRepository,
        ITagRepository tagRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<RemoveTagFromFileCommand, ImageFileViewModel>
    {
        readonly IFileRepository _fileRepository = fileRepository;
        readonly ITagRepository _tagRepository = tagRepository;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;

        public async Task<ImageFileViewModel> Handle(RemoveTagFromFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetAsync(
                [f => f.Id.ToString() == request.FileId],
                [
                    f => f.Uploader,
                    f => f.Tags
                ],
                cancellationToken
            );
            if (file == null)
            {
                throw new Exception();
            }

            var attachedTag = await _tagRepository.GetAsync([t => t.Name == request.Tag], [t => t.Files], cancellationToken);
            if (attachedTag == null)
            {
                attachedTag = _tagRepository.Create(new Tag { Name = request.Tag });
            }
            else if (!file.Tags.Contains(attachedTag))
            {
                throw new Exception();
            }

            file.Tags.Remove(attachedTag);
            _fileRepository.Update(file);
            await _unitOfWork.Save(cancellationToken);

            return _mapper.Map<ImageFileViewModel>(file);
        }
    }
}
