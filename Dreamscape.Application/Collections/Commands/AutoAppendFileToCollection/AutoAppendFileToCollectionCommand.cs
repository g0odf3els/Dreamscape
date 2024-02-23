using MediatR;

namespace Dreamscape.Application.Collections.Commands.AutoAppendFileToCollection
{
    public sealed record AutoAppendFileToCollectionCommand(string UserId, string FileId)
        : IRequest<CollectionViewModel>;
}
