using Dreamscape.Application.Files.Queries;
using Dreamscape.Domain.Entities;
using Pgvector;
using System.Linq.Expressions;

namespace Dreamscape.Application.Repositories
{
    public interface IFileRepository : IBaseRepository<ImageFile>
    {
        PagedList<ImageFile> GetSimilarPagedAsync(
           int pageNumber,
           int pageSize,
           Vector vector,
           Expression<Func<ImageFile, bool>>[]? filter = null,
           Expression<Func<ImageFile, object>>[]? include = null,
           CancellationToken cancellationToken = default);
    }
}
