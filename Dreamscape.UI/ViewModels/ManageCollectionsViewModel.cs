using Dreamscape.Application.Collections;
using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Users;

namespace Dreamscape.UI.ViewModels
{
    public class ManageCollectionsViewModel
    {
        public UserViewModel User { get; set; }

        public PagedList<CollectionViewModel> Collections { get; set; }
    }
}
