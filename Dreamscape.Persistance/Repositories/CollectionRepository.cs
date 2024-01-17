using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using Dreamscape.Persistance.Context;

namespace Dreamscape.Persistance.Repositories
{
    public class CollectionRepository : BaseRepository<Collection>, ICollectionRepository
    {
        public CollectionRepository(DataContext context) : base(context)
        {

        }
    }
}
