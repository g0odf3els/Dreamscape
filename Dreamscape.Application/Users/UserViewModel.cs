namespace Dreamscape.Application.Users
{
    public sealed record UserViewModel
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string UserProfileImagePath { get; set; }
    }
}
