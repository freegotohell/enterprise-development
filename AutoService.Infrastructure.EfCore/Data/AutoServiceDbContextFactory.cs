using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AutoService.Infrastructure.EfCore.Data;

/// <summary>
/// Factory for creating AutoServiceDbContext instances for EF Core tools
/// </summary>
public class AutoServiceDbContextFactory : IDesignTimeDbContextFactory<AutoServiceDbContext>
{
    public AutoServiceDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("AUTOSERVICE_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("The environment variable 'AUTOSERVICE_CONNECTION_STRING' is not set");
        }

        var options = new DbContextOptionsBuilder<AutoServiceDbContext>().UseMySQL(connectionString).Options;

        return new AutoServiceDbContext(options);
    }
}
