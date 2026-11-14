using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AutoService.Infrastructure.EfCore.Data;

public class AutoServiceDbContextFactory : IDesignTimeDbContextFactory<AutoServiceDbContext>
{
    public AutoServiceDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("AUTOSERVICE_CONNECTION_STRING")
            ?? "Server=localhost;Database=autoservice;User=root;Password=1;";

        var options = new DbContextOptionsBuilder<AutoServiceDbContext>().UseMySQL(connectionString).Options;

        return new AutoServiceDbContext(options);
    }
}
