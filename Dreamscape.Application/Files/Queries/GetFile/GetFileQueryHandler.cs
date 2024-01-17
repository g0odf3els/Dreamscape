using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;

namespace Dreamscape.Application.Files.Queries.GetFile
{
    public class GetFileQueryHandler(
        IFileRepository fileRepository,
        IMapper mapper)
        : IRequestHandler<GetFileQuery, ImageFileViewModel>
    {
        readonly IFileRepository _fileRepository = fileRepository;
        readonly IMapper _mapper = mapper;

        public async Task<ImageFileViewModel> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            var result = await _fileRepository.GetAsync(
                [f => f.Id.ToString() == request.Id],
                [
                    f => f.Resolution,
                    f => f.Uploader,
                    f => f.Tags,
                    f => f.Colors,
                ],
                cancellationToken
            ) ?? throw new NotFoundException(nameof(ImageFile), request.Id);

            return _mapper.Map<ImageFileViewModel>(result);
        }
    }
}
