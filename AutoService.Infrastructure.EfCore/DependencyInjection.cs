using AutoService.Domain.Interfaces.Repositories;
using AutoService.Infrastructure.EfCore.Data;
using AutoService.Infrastructure.EfCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService.Infrastructure.EfCore;

public static class DependencyInjection
{
    public static IServiceCollection AddEfCoreInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AutoServiceDbContext>(options => options.UseMySQL(connectionString));

        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IMechanicRepository, MechanicRepository>();
        services.AddScoped<IWorkTypeRepository, WorkTypeRepository>();
        services.AddScoped<IRepairOrderRepository, RepairOrderRepository>();

        return services;
    }
}
