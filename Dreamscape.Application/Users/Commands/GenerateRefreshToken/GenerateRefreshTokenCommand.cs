using MediatR;

namespace Dreamscape.Application.Users.Commands.GenerateRefreshToken
{
    public class GenerateRefreshTokenCommand
        : IRequest<GenerateRefreshTokenView>
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
