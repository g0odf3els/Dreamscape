using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using Dreamscape.Persistance.Context;

namespace Dreamscape.Persistance.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(DataContext context) : base(context)
        {
        }
    }
}
