using FluentValidation;

namespace Dreamscape.Application.Collections.Queries.GetPagedCollections
{
    public class GetPagedCollectionsQueryValidator : AbstractValidator<GetPagedCollectionsQuery>
    {
        public GetPagedCollectionsQueryValidator()
        {
            RuleFor(x => x.Page).Must(pageNumber => pageNumber > 0).WithMessage("Page number have to be grater than zero.");
            RuleFor(x => x.PageSize).Must(pageSize => pageSize > 0).WithMessage("Page size have to be grater than zero.");
        }
    }
}
