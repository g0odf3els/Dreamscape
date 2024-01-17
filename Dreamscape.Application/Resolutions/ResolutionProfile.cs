using AutoMapper;
using Dreamscape.Application.Resolutions.Commands.CreateResolution;
using Dreamscape.Domain.Entities;

namespace Dreamscape.Application.Resolutions
{
    public class ResolutionProfile : Profile
    {
        public ResolutionProfile()
        {
            CreateMap<Resolution, ResolutionViewModel>();

            CreateMap<CreateResolutionCommand, Resolution>();
        }
    }
}
