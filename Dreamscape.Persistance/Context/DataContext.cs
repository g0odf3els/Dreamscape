using Dreamscape.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dreamscape.Persistance.Context
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<ImageFile> ImageFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("vector");

            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.Id });
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

    }
}
