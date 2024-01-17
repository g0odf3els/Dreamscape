using Dreamscape.Application.Collections;
using Dreamscape.Application.Files;
using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Users;

namespace Dreamscape.UI.ViewModels
{
    public class UserProfileViewModel
    {
        public UserViewModel User { get; set; }

        public PagedList<ImageFileViewModel> RecentUploads { get; set; }

        public PagedList<CollectionViewModel> RecentCollections { get; set; }
    }
}
