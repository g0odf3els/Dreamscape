using Dreamscape.Application.Files.Queries.GetPagedFiles;
using MediatR;
using Pgvector;

namespace Dreamscape.Application.Files.Queries.GetSimilarPagedFiles
{
    public class GetSimilarPagedFilesQuery : GetPagedFilesQuery, IRequest<PagedList<ImageFileViewModel>>
    {
        public  string FileId { get; set; }
    }
}
