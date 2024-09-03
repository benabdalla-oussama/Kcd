using Kcd.Domain;
using Kcd.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using System.Data;

namespace Kcd.Persistence.DatabaseContext
{
    public class UserApplicationDatabaseContext(DbContextOptions<UserApplicationDatabaseContext> options, ISystemClock clock) : DbContext(options)
    {
        private readonly ISystemClock _clock = clock;

        public DbSet<UserApplication> UserApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserApplicationDatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
                .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            {
                entry.Entity.DateModified = _clock.UtcNow.DateTime;
                //entry.Entity.ModifiedBy = _userService.UserId;
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = _clock.UtcNow.DateTime;
                    //entry.Entity.CreatedBy = _userService.UserId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
