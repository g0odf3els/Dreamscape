using FluentValidation;

namespace Dreamscape.Application.Resolutions.Commands.CreateResolution
{
    public class CreateResolutionCommandValidator : AbstractValidator<CreateResolutionCommand>
    {
        public CreateResolutionCommandValidator()
        {
            RuleFor(x => x.Width).GreaterThan(0);
            RuleFor(x => x.Height).GreaterThan(0);
        }
    }
}
