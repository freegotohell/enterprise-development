using AutoService.Domain.Shared.Enums;
using AutoService.Tests.Fixtures;
using AutoService.Api.Services;
using Microsoft.Extensions.Logging.Abstractions;
using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Contracts.DTOs;

namespace AutoService.Tests;

/// <summary>
/// Contains tests for LINQ queries over the AutoService domain model.
/// </summary>
public class QueriesTests(AutoServiceFixture fixture) : IClassFixture<AutoServiceFixture>
{
    /// <summary>
    /// Verifies that mechanics can be filtered by their specialization through the service
    /// </summary>
    [Fact]
    public void ReturnMechanicsBySpecialization()
    {
        AutoServiceContext context = fixture.Context;
        MechanicSpecialization specialization = MechanicSpecialization.Engine;

        var expected = context.Mechanics
            .Where(mechanic => mechanic.Specialization == specialization)
            .Select(mechanic => mechanic.Id)
            .OrderBy(id => id)
            .ToList();

        var service = new MechanicService(context, NullLogger<MechanicService>.Instance);

        var actual = service.GetBySpecialization(specialization).Select(mechanic => mechanic.Id).OrderBy(id => id).ToList();

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that clients associated with a mechanic can be retrieved and sorted by name through the service
    /// </summary>
    [Fact]
    public void ClientsByMechanic()
    {
        AutoServiceContext context = fixture.Context;

        Mechanic mechanic = context.Mechanics.First();

        var expected = context.RepairOrders
            .Where(order => order.Mechanics.Any(orderMechanic => orderMechanic.MechanicId == mechanic.Id))
            .Select(order => order.Client)
            .Distinct()
            .OrderBy(client => client.FullName)
            .Select(client => client.Id)
            .ToList();

        var service = new MechanicService(context, NullLogger<MechanicService>.Instance);

        var actual = service.GetClients(mechanic.Id)!
            .OrderBy(client => client.FullName)
            .Select(client => client.Id)
            .ToList();

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that clients with more than one repair request during the last month are returned through the service
    /// </summary>
    [Fact]
    public void ReturnClientsWithRepeatedRequestsLastMonth()
    {
        AutoServiceContext context = fixture.Context;
        DateTime monthAgo = DateTime.Now.AddMonths(-1);

        var expected = context.RepairOrders
            .Where(order => order.AdmissionDate >= monthAgo)
            .GroupBy(order => order.ClientId)
            .Where(group => group.Count() > 1)
            .Select(group => new
            {
                ClientId = group.Key,
                RequestsCount = group.Count()
            })
            .OrderBy(x => x.ClientId)
            .ToList();

        var service = new ClientService(context, NullLogger<ClientService>.Instance);

        var actual = service.GetRepeatedLastMonth()
            .Select(client => new
            {
                ClientId = client.ClientId,
                RequestsCount = client.RequestsCount
            })
            .OrderBy(x => x.ClientId)
            .ToList();

        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that the total cost of an order is calculated from its associated work items through the service
    /// </summary>
    [Fact]
    public void TotalCostForOrder()
    {
        AutoServiceContext context = fixture.Context;
        RepairOrder order = context.RepairOrders.First();

        var expected = order.Works.Sum(work => work.WorkType.Cost);

        var service = new RepairOrderService(context, NullLogger<RepairOrderService>.Instance);

        RepairOrderCostDto? actual = service.GetTotalCost(order.Id);

        Assert.NotNull(actual);
        Assert.Equal(order.Id, actual.RepairOrderId);
        Assert.Equal(expected, actual.TotalCost);
    }

    /// <summary>
    /// Verifies that the five most frequently performed work types are returned in descending order through the service
    /// </summary>
    [Fact]
    public void Top5MostFrequentWorkTypes()
    {
        AutoServiceContext context = fixture.Context;

        var expected = context.RepairOrders
            .SelectMany(order => order.Works)
            .GroupBy(orderWork => orderWork.WorkTypeId)
            .Select(group => new
            {
                WorkTypeId = group.Key,
                Count = group.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        var service = new WorkTypeService(
            context,
            NullLogger<WorkTypeService>.Instance);

        var actual = service.GetTop5MostFrequent()
            .Select(work => new
            {
                WorkTypeId = work.WorkTypeId,
                Count = work.Count
            })
            .ToList();

        Assert.Equal(expected, actual);
    }
}
