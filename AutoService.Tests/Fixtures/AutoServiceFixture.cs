using AutoService.Application.Services;
using AutoService.Contracts.Services;
using AutoService.Domain.Data;
using AutoService.Infrastructure.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace AutoService.Tests.Fixtures;

/// <summary>
/// Provides configured application services for unit tests
/// </summary>
public class AutoServiceFixture
{
    private readonly ServiceProvider _serviceProvider;

    /// <summary>
    /// Gets the in-memory context containing seeded AutoService data
    /// </summary>
    public AutoServiceContext Context => _serviceProvider.GetRequiredService<AutoServiceContext>();

    /// <summary>
    /// Initializes the test fixture and configures application services
    /// </summary>
    public AutoServiceFixture()
    {
        ServiceCollection services = new();

        services.AddInMemoryInfrastructure();

        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ICarService, CarService>();
        services.AddScoped<IMechanicService, MechanicService>();
        services.AddScoped<IWorkTypeService, WorkTypeService>();
        services.AddScoped<IRepairOrderService, RepairOrderService>();

        services.AddLogging();

        _serviceProvider = services.BuildServiceProvider();
    }

    /// <summary>
    /// Gets a configured application service
    /// </summary>
    public T GetService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}