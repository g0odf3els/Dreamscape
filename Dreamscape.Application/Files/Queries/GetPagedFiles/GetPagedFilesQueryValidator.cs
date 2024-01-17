using FluentValidation;

namespace Dreamscape.Application.Files.Queries.GetPagedFiles
{
    public class GetPagedFilesQueryValidator : AbstractValidator<GetPagedFilesQuery>
    {
        public GetPagedFilesQueryValidator()
        {
            RuleFor(x => x.Page).Must(pageNumber => pageNumber > 0).WithMessage("Page number have to be grater than zero.");
            RuleFor(x => x.PageSize).Must(pageSize => pageSize > 0).WithMessage("Page size have to be grater than zero.");
        }
    }
}
