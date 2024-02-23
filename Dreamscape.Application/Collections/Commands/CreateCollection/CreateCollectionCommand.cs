using MediatR;

namespace Dreamscape.Application.Collections.Commands.CreateCollection
{
    public record CreateCollectionCommand(string UserId,
                                          string Name,
                                          string Description,
                                          bool IsPrivate = false,
                                          string[]? FilesId = null)
        : IRequest<CollectionViewModel>;
}
