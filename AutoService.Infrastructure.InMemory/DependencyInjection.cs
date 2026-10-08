using AutoService.Domain.Data;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Infrastructure.InMemory.Data;
using AutoService.Infrastructure.InMemory.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService.Infrastructure.InMemory;

public static class DependencyInjection
{
    public static IServiceCollection AddInMemoryInfrastructure(this IServiceCollection services)
    {
        AutoServiceContext context = DataSeeder.Seed();

        services.AddSingleton(context);

        services.AddSingleton<IClientRepository, ClientRepository>();
        services.AddSingleton<ICarRepository, CarRepository>();
        services.AddSingleton<IMechanicRepository, MechanicRepository>();
        services.AddSingleton<IWorkTypeRepository, WorkTypeRepository>();
        services.AddSingleton<IRepairOrderRepository, RepairOrderRepository>();

        return services;
    }
}