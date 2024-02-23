using AutoMapper;
using Dreamscape.Application.Common.Exceptions;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using MediatR;
using Pgvector.EntityFrameworkCore;

namespace Dreamscape.Application.Files.Queries.GetSimilarPagedFiles
{
    public class GetSimilarPagedFilesQueryHandler(
        IFileRepository fileRepository,
        IMapper mapper)
        : IRequestHandler<GetSimilarPagedFilesQuery, PagedList<ImageFileViewModel>>
    {
        readonly IFileRepository _fileRepository = fileRepository;
        readonly IMapper _mapper = mapper;

        public async Task<PagedList<ImageFileViewModel>> Handle(GetSimilarPagedFilesQuery request, CancellationToken cancellationToken)
        {
            var file = await _fileRepository.GetAsync(
               [f => f.Id.ToString() == request.FileId],
               null,
               cancellationToken
           ) ?? throw new NotFoundException(nameof(ImageFile), request.FileId);

            if (file.Vector == null)
            {
                throw new ArgumentException($"File {file.Id} vector is null");
            }

            var files = await _fileRepository.GetPagedAsync(
                pageNumber: request.Page,
                pageSize: request.PageSize,
                predicate: [f => f.Id != file.Id],
                orderBy: x => x.Vector!.L2Distance(file.Vector),
                include:[
                     f => f.Resolution,
                     f => f.Uploader,
                     f => f.Colors
                 ],
               cancellationToken: cancellationToken);

            return _mapper.Map<PagedList<ImageFileViewModel>>(files);
        }
    }
}
