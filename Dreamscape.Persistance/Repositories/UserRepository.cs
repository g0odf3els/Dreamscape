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

        public async Task<User?> GetAsync(Expression<Func<User, bool>>[]? filters, Expression<Func<User, object>>[]? includes, CancellationToken cancellationToken)
        {
            var query = BuildQuery(filters, includes);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByUsername(string username, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllAsync(Expression<Func<User, bool>>[]? filters = null, Expression<Func<User, object>>? orderBy = null, bool? orderByDescending = true, Expression<Func<User, object>>[]? includes = null, int? count = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<User>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<User, bool>>[]? filter = null, Expression<Func<User, object>>? orderBy = null, bool? orderByDescending = true, Expression<Func<User, object>>[]? include = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private IQueryable<User> BuildQuery(Expression<Func<User, bool>>[]? filters, Expression<Func<User, object>>[]? includes)
        {
            var query = Context.Set<User>().AsQueryable();

            if (includes != null && includes.Length != 0)
            {
                query = includes.Aggregate(query, (current, property) => current.Include(property));
            }

            if (filters != null && filters.Length != 0)
            {
                var combinedFilter = PredicateBuilder.New<User>(true);
                query = filters.Aggregate(query, (current, filter) => current.Where(filter));
            }

            return query;
        }

        public Task<PagedList<User>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<User, bool>>[]? filter = null, Expression<Func<User, double>>? orderBy = null, bool? orderByDescending = true, Expression<Func<User, object>>[]? include = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
