using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace Dreamscape.Application.Files.Queries.GetSimilarPagedFiles
{
    public class GetSimilarPagedFilesQueryHandler(
        IFileRepository fileRepository,
        IMapper mapper)
        : IRequestHandler<GetSimilarPagedFilesQuery, PagedList<ImageFileViewModel>>
    {
        readonly IFileRepository _fileRepository  = fileRepository;
        readonly IMapper _mapper = mapper;

        public async Task<PagedList<ImageFileViewModel>> Handle(GetSimilarPagedFilesQuery request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetAsync(
               [f => f.Id.ToString() == request.FileId],
               null,
               cancellationToken
           ) ?? throw new NotFoundException(nameof(ImageFile), request.FileId);

            var files = _fileRepository.GetSimilarPagedAsync(
                   request.Page,
                   request.PageSize,
                   file.Vector,
                   null,
                   [
                       f => f.Resolution,
                       f => f.Uploader,
                       f => f.Colors
                   ],
                   cancellationToken
               );

            return _mapper.Map<PagedList<ImageFileViewModel>>(files);
        }
    }
}
