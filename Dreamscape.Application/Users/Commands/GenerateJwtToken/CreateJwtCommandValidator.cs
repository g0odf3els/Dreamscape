using FluentValidation;

namespace Dreamscape.Application.Users.Commands.GenerateJwtToken
{
    public sealed class CreateJwtCommandValidator : AbstractValidator<CreateJwtCommand>
    {
        public CreateJwtCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().MinimumLength(8).MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(50);
        }
    }
}
