using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Users;

namespace Dreamscape.UI.ViewModels
{
    public class UserProfileViewModel<T>
    {
        public UserViewModel User { get; set; }

        public PagedList<T> Items { get; set; }
    }
}
