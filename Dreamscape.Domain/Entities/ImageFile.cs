using Dreamscape.Domain.Common;
using Pgvector;

namespace Dreamscape.Domain.Entities
{
    public class ImageFile : BaseEntity
    {
        public ImageFile()
        {
            Tags = new List<Tag>();

            Colors = new List<Color>();

            Collections = new List<Collection>();
        }

        public string Name { get; set; }

        public string DisplaySizePath { get; set; }
        public string FullSizePath { get; set; }

        public string UploaderId { get; set; }
        public User Uploader { get; set; }

        public Guid ResolutionId { get; set; }
        public Resolution Resolution { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Color> Colors { get; set; }

        public List<Collection> Collections { get; set; }

        public Vector? Vector { get; set; }

        public double Length { get; set; }
    }
}
