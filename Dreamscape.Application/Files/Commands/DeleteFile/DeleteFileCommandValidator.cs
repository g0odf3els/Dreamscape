using FluentValidation;


namespace Dreamscape.Application.Files.Commands.DeleteFile
{
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User id can't be empty string.");
            RuleFor(x => x.FileId).NotEmpty().WithMessage("File id can't be empty string.");
        }
    }
}
