using AutoMapper;
using Dreamscape.Domain.Entities;

namespace Dreamscape.Application.Colors
{
    public class ColorProfile : Profile
    {
        public ColorProfile()
        {
            CreateMap<Color, ColorViewModel>();
        }
    }
}
