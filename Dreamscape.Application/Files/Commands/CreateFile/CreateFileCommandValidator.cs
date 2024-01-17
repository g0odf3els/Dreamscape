using FluentValidation;
using Microsoft.AspNetCore.Http;


namespace Dreamscape.Application.Files.Commands.CreateFile
{
    public class CreateFileCommandValidator : AbstractValidator<CreateFileCommand>
    {
        private readonly List<string> AllowedImageExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".bmp" };
        private readonly List<string> AllowedImageMimeTypes = new List<string> { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp" };

        public CreateFileCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User id can't be empty string.");

            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.")
                .Must(file => file.Length > 0).WithMessage("File cannot be empty.")
                .Must(IsValidImageType).WithMessage("Invalid image type.");
        }

        private bool IsValidImageType(IFormFile file)
        {
            if (file == null)
            {
                return false;
            }

            var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            var fileMimeType = file.ContentType.ToLowerInvariant();

            return AllowedImageExtensions.Contains(fileExtension) && AllowedImageMimeTypes.Contains(fileMimeType);
        }
    }
}
