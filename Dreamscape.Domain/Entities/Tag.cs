using Dreamscape.Domain.Common;

namespace Dreamscape.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public Tag()
        {
            Files = new List<ImageFile>();
        }

        public required string Name { get; set; }

        public List<ImageFile> Files { get; set; }
    }
}
