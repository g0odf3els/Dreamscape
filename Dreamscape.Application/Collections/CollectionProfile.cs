using AutoMapper;
using Dreamscape.Domain.Entities;

namespace Dreamscape.Application.Collections
{
    public class CollectionProfile : Profile
    {
        public CollectionProfile()
        {
            CreateMap<Collection, CollectionViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();
        }
    }
}
