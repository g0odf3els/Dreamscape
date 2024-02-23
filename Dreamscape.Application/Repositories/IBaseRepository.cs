using Dreamscape.Application.Files.Queries;
using System.Linq.Expressions;

namespace Dreamscape.Application.Repositories
{
    public interface IBaseRepository<T>
    {
        T Create(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<T?> GetAsync(
            Expression<Func<T, bool>>[]? predicate,
            Expression<Func<T, object>>[]? include = null,
            CancellationToken cancellationToken = default);

        Task<PagedList<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>[]? predicate = null,
            Expression<Func<T, object>>? orderBy = null,
            Expression<Func<T, object>>[]? include = null,
            bool? orderByDescending = false,
            CancellationToken cancellationToken = default);

    }
}

