using MediatR;

namespace Dreamscape.Application.Files.Queries.IsInUserCollection
{
    public sealed record IsInUserCollectionQuery(string UserId, string FileId)
        : IRequest<bool>;
}
