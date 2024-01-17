using AutoMapper;
using Dreamscape.Application.Files.Queries;
using Dreamscape.Domain.Entities;

namespace Dreamscape.Application.Collections.Queries.GetPagedCollections
{
    public class GetPagedCollectionsProfile : Profile
    {
        public GetPagedCollectionsProfile()
        {
            CreateMap<PagedList<Collection>, PagedList<CollectionViewModel>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
