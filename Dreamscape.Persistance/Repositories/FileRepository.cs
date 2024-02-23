using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using Dreamscape.Persistance.Context;


namespace Dreamscape.Persistance.Repositories
{
    public class FileRepository(DataContext context) : BaseRepository<ImageFile>(context), IFileRepository
    {
       
        
    }
}
