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
            Expression<Func<T, bool>>[]? filter,
            Expression<Func<T, object>>[]? include,
            CancellationToken cancellationToken);

        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>[]? filters = null,
            Expression<Func<T, object>>? orderBy = null,
            bool? orderByDescending = true,
            Expression<Func<T, object>>[]? includes = null,
            int? count = null,
            CancellationToken cancellationToken = default);

        Task<PagedList<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>[]? filter = null,
            Expression<Func<T, object>>? orderBy = null,
            bool? orderByDescending = true,
            Expression<Func<T, object>>[]? include = null,
            CancellationToken cancellationToken = default);

    }
}

