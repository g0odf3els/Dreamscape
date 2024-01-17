using FluentValidation;

namespace Dreamscape.Application.Files.Commands.AddTagToFile
{
    public class AddTagToFileCommandValidator : AbstractValidator<AddTagToFileCommand>
    {
        public AddTagToFileCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User id can't be empty string.");
            RuleFor(x => x.FileId).NotEmpty().WithMessage("File id can't be empty string.");
            RuleFor(x => x.Tag).NotEmpty().WithMessage("Tag can't be empty string.");
        }
    }
}
