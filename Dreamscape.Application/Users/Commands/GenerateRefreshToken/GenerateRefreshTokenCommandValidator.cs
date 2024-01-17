using FluentValidation;

namespace Dreamscape.Application.Users.Commands.GenerateRefreshToken
{
    public class GenerateRefreshTokenCommandValidator : AbstractValidator<GenerateRefreshTokenCommand>
    {
        public GenerateRefreshTokenCommandValidator()
        {
        }
    }
}
