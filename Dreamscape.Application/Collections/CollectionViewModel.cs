using Dreamscape.Application.Files;
using Dreamscape.Application.Tags;
using Dreamscape.Application.Users;

namespace Dreamscape.Application.Collections
{
    public class CollectionViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsPrivate { get; set; }

        public DateTimeOffset DataCreated { get; set; }

        public UserViewModel Owner { get; set; }

        public List<ImageFileViewModel> Files { get; set; }

        public List<TagViewModel> Tags { get; set; }
    }
}
