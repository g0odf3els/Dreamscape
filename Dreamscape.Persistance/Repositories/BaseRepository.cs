using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Common;
using Dreamscape.Persistance.Context;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dreamscape.Persistance.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DataContext Context;

        public BaseRepository(DataContext context)
        {
            Context = context;
        }

        public T Create(T entity)
        {
            return Context.Add(entity).Entity;
        }

        public void Update(T entity)
        {
            Context.Update(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>>[]? predicate,
            Expression<Func<T, object>>[]? include = null,
            CancellationToken cancellationToken = default)
        {
            var query = Context.Set<T>().AsQueryable();

            if (include != null && include.Length != 0)
            {
                query = include.Aggregate(query, (current, property) => current.Include(property));
            }

            if (predicate != null && predicate.Length != 0)
            {
                query = predicate.Aggregate(query, (current, filter) => current.Where(filter));
            }

            return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedList<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>[]? predicate = null,
            Expression<Func<T, object>>? orderBy = null,
            Expression<Func<T, object>>[]? include = null,
            bool? orderByDescending = false,
            CancellationToken cancellationToken = default)
        {
            var query = Context.Set<T>().AsQueryable();

            if (include != null && include.Length != 0)
            {
                query = include.Aggregate(query, (current, property) => current.Include(property));
            }

            if (predicate != null && predicate.Length != 0)
            {
                query = predicate.Aggregate(query, (current, filter) => current.Where(filter));
            }

            if (orderBy != null)
            {
                query = orderByDescending.GetValueOrDefault()
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }

            var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedList<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
