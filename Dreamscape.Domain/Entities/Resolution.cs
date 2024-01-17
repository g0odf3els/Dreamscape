using Dreamscape.Domain.Common;

namespace Dreamscape.Domain.Entities
{
    public class Resolution : BaseEntity
    {
        public Resolution()
        {
            Images = new List<ImageFile>();
        }

        public required int Width { get; set; }
        public required int Height { get; set; }

        public List<ImageFile> Images { get; set; }
    }
}
