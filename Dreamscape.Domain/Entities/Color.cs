using Dreamscape.Domain.Common;

namespace Dreamscape.Domain.Entities
{
    public class Color : BaseEntity
    {
        public Color()
        {
            Images = new List<ImageFile>();
        }

        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public List<ImageFile> Images { get; set; }
    }
}
