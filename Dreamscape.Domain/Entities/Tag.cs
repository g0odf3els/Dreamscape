using Dreamscape.Domain.Common;

namespace Dreamscape.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public Tag()
        {
            Files = new List<ImageFile>();

            Collections = new List<Collection>();
        }

        public required string Name { get; set; }

        public List<ImageFile> Files { get; set; }

        public List<Collection> Collections { get; set; }
    }
}
