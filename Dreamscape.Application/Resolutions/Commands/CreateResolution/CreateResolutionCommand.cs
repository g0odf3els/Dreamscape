using MediatR;

namespace Dreamscape.Application.Resolutions.Commands.CreateResolution
{
    public record CreateResolutionCommand(int Width, int Height)
        : IRequest<ResolutionViewModel>;
}
