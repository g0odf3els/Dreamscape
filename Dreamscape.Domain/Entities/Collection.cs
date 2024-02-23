using Dreamscape.Domain.Common;
using Pgvector;

namespace Dreamscape.Domain.Entities
{
    public class Collection : BaseEntity
    {
        public Collection()
        {
            Files = new List<ImageFile>();

            Tags = new List<Tag>();
        }

        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsPrivate { get; set; }

        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public List<ImageFile> Files { get; set; }

        public List<Tag> Tags { get; set; }

        public Vector? Vector { get; set; }

    }
}
