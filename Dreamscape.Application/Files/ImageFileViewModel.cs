using Dreamscape.Application.Colors;
using Dreamscape.Application.Resolutions;
using Dreamscape.Application.Tags;
using Dreamscape.Application.Users;

namespace Dreamscape.Application.Files
{
    public class ImageFileViewModel
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public DateTimeOffset DataCreated { get; set; }
        public DateTimeOffset DataUpdated { get; set; }
        public DateTimeOffset DataDeleted { get; set; }

        public required string DisplaySizePath { get; set; }
        public required string FullSizePath { get; set; }

        public required string UploaderId { get; set; }
        public UserViewModel Uploader { get; set; }

        public required ResolutionViewModel Resolution { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; } = [];

        public IEnumerable<ColorViewModel> Colors { get; set; } = [];

        public double Length { get; set; }
    }
}
