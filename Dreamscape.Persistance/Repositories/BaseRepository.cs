using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Common;
using Dreamscape.Persistance.Context;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dreamscape.Persistance.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DataContext Context;

        public BaseRepository(DataContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
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
            Expression<Func<T, bool>>[]? filters,
            Expression<Func<T, object>>[]? include,
            CancellationToken cancellationToken)
        {
            var query = BuildQuery(filters, include);
            return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>[]? filters = null,
            Expression<Func<T, object>>? orderBy = null,
            bool? orderByDescending = true,
            Expression<Func<T, object>>[]? includes = null,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(filters, includes);
            query = ApplyOrder(query, orderBy, orderByDescending);
            query = ApplyCount(query, count);

            return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<PagedList<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>[]? filter = null,
            Expression<Func<T, object>>? orderBy = null,
            bool? orderByDescending = true,
            Expression<Func<T, object>>[]? include = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(filter, include);
            query = ApplyOrder(query, orderBy, orderByDescending);

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

        protected IQueryable<T> BuildQuery(Expression<Func<T, bool>>[]? filters, Expression<Func<T, object>>[]? includes)
        {
            var query = Context.Set<T>().AsQueryable();

            if (includes != null && includes.Length != 0)
            {
                query = includes.Aggregate(query, (current, property) => current.Include(property));
            }

            if (filters != null && filters.Length != 0)
            {
                var combinedFilter = PredicateBuilder.New<T>(true);
                query = filters.Aggregate(query, (current, filter) => current.Where(filter));
            }

            return query;
        }

        protected IQueryable<T> ApplyOrder(IQueryable<T> query, Expression<Func<T, object>>? orderBy, bool? orderByDescending)
        {
            if (orderBy != null)
            {
                query = query = orderByDescending ?? true
                ? query.OrderByDescending(orderBy)
                : query.OrderBy(orderBy);
            }

            return query;
        }

        protected IQueryable<T> ApplyCount(IQueryable<T> query, int? count)
        {
            if (count != null)
            {
                query = query.Take(count.Value);
            }

            return query;
        }
    }
}