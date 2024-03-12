using MediatR;

namespace Dreamscape.Application.Files.Queries.GetPagedFiles
{
    public class GetPagedFilesQuery : IRequest<PagedList<ImageFileViewModel>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 16;
        public string? Search { get; set; }
        public string? Resolutions { get; set; }
        public string? AspectRatios { get; set; }
        public string? UploaderId { get; set; }
        public string? UserId { get; set; }
        public string? CollectionId { get; set; }
        public int Order { get; set; } = 0;
        public bool OrderByDescending = true;
    }
}
