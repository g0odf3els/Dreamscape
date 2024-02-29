using Dreamscape.Application.Files;
using Dreamscape.Application.Files.Queries;

namespace Dreamscape.UI.ViewModels
{
    public class IndexViewModel
    {
        public PagedList<ImageFileViewModel>? RecentUploaded { get; set; }
    }
}
