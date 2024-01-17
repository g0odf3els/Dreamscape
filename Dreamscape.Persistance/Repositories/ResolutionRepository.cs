using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using Dreamscape.Persistance.Context;
namespace Dreamscape.Persistance.Repositories
{
    public class ResolutionRepository : BaseRepository<Resolution>, IResolutionRepository
    {
        public ResolutionRepository(DataContext context) : base(context)
        {
        }
    }
}
