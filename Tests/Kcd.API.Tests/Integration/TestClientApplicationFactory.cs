using Kcd.Identity.DatabaseContext;
using Kcd.Persistence.DatabaseContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kcd.API.Tests.Integration;

public sealed class TestClientApplicationFactory<TEntryPoint>(string msqlConnectionString) : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace IdentityDatabaseContext with the Testcontainers SQL Server
            var identityDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<IdentityDatabaseContext>));
            if (identityDescriptor != null)
            {
                services.Remove(identityDescriptor);
            }

            services.AddDbContext<IdentityDatabaseContext>(options =>
            {
                options.UseSqlServer(msqlConnectionString);
            });

            // Replace UserApplicationDatabaseContext with in-memory SQLite
            var appDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<UserApplicationDatabaseContext>));
            if (appDescriptor != null)
            {
                services.Remove(appDescriptor);
            }

            services.AddDbContext<UserApplicationDatabaseContext>(options =>
            {
                options.UseSqlite("Data Source=./database.test.sqlite");
            });

            // Apply migrations and seed data for both contexts
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();

            // Apply migrations to the SQL Server context
            var identityDb = scope.ServiceProvider.GetRequiredService<IdentityDatabaseContext>();
            identityDb.Database.Migrate();

            // Apply migrations to the SQLite context
            var userAppDb = scope.ServiceProvider.GetRequiredService<UserApplicationDatabaseContext>();
            userAppDb.Database.OpenConnection();
            userAppDb.Database.Migrate();
            userAppDb.Database.EnsureCreated();
        });
    }
}
