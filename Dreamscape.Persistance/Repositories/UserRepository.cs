using Dreamscape.Application.Files.Queries;
using Dreamscape.Application.Repositories;
using Dreamscape.Domain.Entities;
using Dreamscape.Persistance.Context;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dreamscape.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly DataContext Context;

        public UserRepository(DataContext context)
        {
            Context = context;
        }

        public User Create(User entity)
        {
            return Context.Add(entity).Entity;
        }

        public void Update(User entity)
        {
            Context.Update(entity);
        }

        public void Delete(User entity)
        {
            entity.DataDeleted = DateTimeOffset.UtcNow;
            Context.Update(entity);
        }

        public async Task<User?> GetAsync(Expression<Func<User, bool>>[]? predicate, Expression<Func<User, object>>[]? include, CancellationToken cancellationToken)
        {
            var query = Context.Set<User>().AsQueryable();

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

        public Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByUsername(string username, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<User>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<User, bool>>[]? predicate = null, Expression<Func<User, object>>? orderBy = null, Expression<Func<User, object>>[]? include = null, bool? orderByDescending = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
