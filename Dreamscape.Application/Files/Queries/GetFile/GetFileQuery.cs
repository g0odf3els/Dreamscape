using MediatR;

namespace Dreamscape.Application.Files.Queries.GetFile
{
    public record GetFileQuery(string Id)
        : IRequest<ImageFileViewModel>;
}
