using AutoMapper;
using Dreamscape.Application.Files.Queries;
using Dreamscape.Domain.Entities;

namespace Dreamscape.Application.Files
{
    internal class ImageFileProfile : Profile
    {
        public ImageFileProfile()
        {
            CreateMap<ImageFile, ImageFileViewModel>();

            CreateMap<PagedList<ImageFile>, PagedList<ImageFileViewModel>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
