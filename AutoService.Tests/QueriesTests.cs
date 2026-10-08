using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
using AutoService.Domain.Entities;
using AutoService.Domain.Shared.Enums;
using AutoService.Tests.Fixtures;

namespace AutoService.Tests;

/// <summary>
/// Contains tests for application services
/// </summary>
public class QueriesTests(AutoServiceFixture fixture) : IClassFixture<AutoServiceFixture>
{
    /// <summary>
    /// Verifies that mechanics can be filtered by their specialization through the service
    /// </summary>
    [Fact]
    public async Task ReturnMechanicsBySpecialization()
    {
        MechanicSpecialization specialization = MechanicSpecialization.Engine;

        IMechanicService service = fixture.GetService<IMechanicService>();

        List<MechanicDto> actual = await service.GetBySpecializationAsync(specialization);

        Assert.NotEmpty(actual);
        Assert.All(actual, mechanic => Assert.Equal(specialization, mechanic.Specialization));
    }

    /// <summary>
    /// Verifies that clients associated with a mechanic can be retrieved and sorted by name through the service
    /// </summary>
    [Fact]
    public async Task ClientsByMechanic()
    {
        Mechanic mechanic = fixture.Context.Mechanics.First();

        var expectedClientIds = fixture.Context.RepairOrders
            .Where(order => order.Mechanics.Any(orderMechanic => orderMechanic.MechanicId == mechanic.Id))
            .Select(order => order.ClientId)
            .Distinct()
            .OrderBy(id => id)
            .ToList();

        IMechanicService service = fixture.GetService<IMechanicService>();

        List<ClientDto>? actual = await service.GetClientsAsync(mechanic.Id);

        Assert.NotNull(actual);

        var actualClientIds = actual.Select(client => client.Id).OrderBy(id => id).ToList();

        Assert.Equal(expectedClientIds, actualClientIds);
    }

    /// <summary>
    /// Verifies that clients with more than one repair request during the last month are returned through the service
    /// </summary>
    [Fact]
    public async Task ReturnClientsWithRepeatedRequestsLastMonth()
    {
        DateTime monthAgo = DateTime.Now.AddMonths(-1);

        var expected = fixture.Context.RepairOrders
            .Where(order => order.AdmissionDate >= monthAgo)
            .GroupBy(order => order.ClientId)
            .Where(group => group.Count() > 1)
            .Select(group => new
            {
                ClientId = group.Key,
                RequestsCount = group.Count()
            })
            .OrderBy(item => item.ClientId)
            .ToList();

        IClientService service = fixture.GetService<IClientService>();

        List<RepeatedClientDto> actual = await service.GetRepeatedLastMonthAsync();

        var actualValues = actual
            .Select(client => new
            {
                ClientId = client.ClientId,
                RequestsCount = client.RequestsCount
            })
            .OrderBy(item => item.ClientId)
            .ToList();

        Assert.Equal(expected, actualValues);
    }

    /// <summary>
    /// Verifies that the total cost of an order is calculated from its associated work items through the service
    /// </summary>
    [Fact]
    public async Task TotalCostForOrder()
    {
        RepairOrder order = fixture.Context.RepairOrders.First();

        var expected = order.Works.Sum(work => work.WorkType.Cost);

        IRepairOrderService service = fixture.GetService<IRepairOrderService>();

        RepairOrderCostDto? actual = await service.GetTotalCostAsync(order.Id);

        Assert.NotNull(actual);
        Assert.Equal(order.Id, actual.RepairOrderId);
        Assert.Equal(expected, actual.TotalCost);
    }

    /// <summary>
    /// Verifies that the five most frequently performed work types are returned in descending order through the service
    /// </summary>
    [Fact]
    public async Task Top5MostFrequentWorkTypes()
    {
        var expected = fixture.Context.RepairOrders
        .SelectMany(order => order.Works)
        .GroupBy(orderWork => orderWork.WorkTypeId)
        .Select(group => new
        {
            WorkTypeId = group.Key,
            Count = group.Count()
        })
        .OrderByDescending(item => item.Count)
        .Take(5)
        .ToList();

        IWorkTypeService service = fixture.GetService<IWorkTypeService>();

        List<FrequentWorkTypeDto> actual = await service.GetTop5MostFrequentAsync();

        var actualValues = actual
            .Select(workType => new
            {
                WorkTypeId = workType.WorkTypeId,
                Count = workType.Count
            })
            .ToList();

        Assert.Equal(expected, actualValues);
    }
}
