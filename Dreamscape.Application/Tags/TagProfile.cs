using AutoMapper;
using Dreamscape.Domain.Entities;

namespace Dreamscape.Application.Tags
{
    internal class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagViewModel>();
        }
    }
}
