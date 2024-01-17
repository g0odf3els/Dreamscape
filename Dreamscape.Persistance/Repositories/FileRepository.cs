using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using Dreamscape.Persistance.Context;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Dreamscape.Persistance.Repositories
{
    public class FileRepository(DataContext context) : BaseRepository<ImageFile>(context), IFileRepository
    {
        public PagedList<ImageFile> GetSimilarPagedAsync(
           int pageNumber,
           int pageSize,
           Vector vector,
           Expression<Func<ImageFile, bool>>[]? filter = null,
           Expression<Func<ImageFile, object>>[]? include = null,
           CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(filter, include);

            var totalCount =  query.Count();
            var items = query.OrderBy(x => x.Vector!.L2Distance(vector)).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<ImageFile> 
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
