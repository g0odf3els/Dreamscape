using Dreamscape.Domain.Entities;


namespace Dreamscape.Application.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
        Task<User?> GetByUsername(string username, CancellationToken cancellationToken);
    }
}
