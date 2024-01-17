using Dreamscape.Domain.Common;

namespace Dreamscape.Domain.Entities
{
    public class Collection : BaseEntity
    {
        public Collection()
        {
            Files = new List<ImageFile>();
        }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public List<ImageFile> Files { get; set; }
    }
}
