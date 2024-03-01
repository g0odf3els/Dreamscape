using Microsoft.AspNetCore.Identity;

namespace Dreamscape.Domain.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            UploadedFiles = new List<ImageFile>();

            Collections = new List<Collection>();
        }

        public DateTimeOffset DataCreated { get; set; }
        public DateTimeOffset DataUpdated { get; set; }
        public DateTimeOffset DataDeleted { get; set; }

        public string? UserProfileImagePath { get; set; }

        public List<ImageFile> UploadedFiles { get; set; }

        public List<Collection> Collections { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }


    }
}
