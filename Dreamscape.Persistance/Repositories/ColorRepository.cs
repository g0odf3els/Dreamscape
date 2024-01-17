using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using Dreamscape.Persistance.Context;

namespace Dreamscape.Persistance.Repositories
{
    public class ColorRepository : BaseRepository<Color>, IColorRepository
    {
        public ColorRepository(DataContext context) : base(context)
        {

        }
    }
}
