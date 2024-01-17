using FluentValidation;

namespace Dreamscape.Application.Users.Commands.CreateUser
{
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            //RuleFor(x => x.Email).NotEmpty().MaximumLength(50).EmailAddress();
            //RuleFor(x => x.Username).NotEmpty().MinimumLength(8).MaximumLength(50);
            //RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(50);
        }
    }
}
