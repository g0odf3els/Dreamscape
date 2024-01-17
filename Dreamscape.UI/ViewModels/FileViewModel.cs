using Dreamscape.Application.Files;
using Dreamscape.Application.Files.Queries;

namespace Dreamscape.UI.ViewModels
{
    public class FileViewModel
    {
        public required ImageFileViewModel Image { get; set; }

        public PagedList<ImageFileViewModel>? SimilarImages { get; set; }

        public bool IsInUserCollection { get; set; }
    }
}
