namespace Dreamscape.Application.Users.Commands.GenerateJwtToken
{
    public class CreateJwtCommandView
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
