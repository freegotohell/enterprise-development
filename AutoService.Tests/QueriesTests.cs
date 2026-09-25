using AutoService.Domain.Shared.Enums;
using AutoService.Tests.Fixtures;

namespace AutoService.Tests;

/// <summary>
/// Contains tests for LINQ queries over the AutoService domain model.
/// </summary>
public class QueriesTests(AutoServiceFixture fixture)
: IClassFixture<AutoServiceFixture>
{
    /// <summary>
    /// Verifies that mechanics can be filtered by their specialization.
    /// </summary>
    [Fact]
    public void ReturnMechanicsByWorkType()
    {
        var context = fixture.Context;
        var specialization = MechanicSpecialization.Engine;

        var mechanics = context.Mechanics
            .Where(mechanic => mechanic.Specialization == specialization)
            .ToList();

        Assert.NotEmpty(mechanics);

        Assert.All(mechanics, mechanic => Assert.Equal(specialization, mechanic.Specialization));
    }

    /// <summary>
    /// Verifies that clients associated with a mechanic can be retrieved and sorted by name.
    /// </summary>
    [Fact]
    public void ReturnClientsByMechanic()
    {
        var context = fixture.Context;

        var mechanic = context.Mechanics.First();

        var clients = context.RepairOrders
            .Where(order => order.Mechanics.Any(orderMechanic => orderMechanic.MechanicId == mechanic.Id))
            .Select(order => order.Client)
            .Distinct()
            .OrderBy(client => client.FullName)
            .ToList();

        Assert.NotEmpty(clients);

        Assert.True(clients.SequenceEqual(clients.OrderBy(x => x.FullName)));
    }

    /// <summary>
    /// Verifies that clients with more than one repair request during the last month are returned.
    /// </summary>
    [Fact]
    public void CountRepeatedClientRequestsLastMonth()
    {
        var context = fixture.Context;
        var monthAgo = DateTime.Now.AddMonths(-1);

        var repeatedClients = context.RepairOrders
            .Where(order => order.AdmissionDate >= monthAgo)
            .GroupBy(order => order.ClientId)
            .Where(group => group.Count() > 1)
            .Select(group => new
            {
                ClientId = group.Key,
                RequestsCount = group.Count()
            })
            .ToList();

        Assert.NotEmpty(repeatedClients);

        Assert.All(repeatedClients, client => Assert.True(client.RequestsCount > 1));
    }

    /// <summary>
    /// Verifies that the total cost of an order is calculated from its associated work items.
    /// </summary>
    [Fact]
    public void CalculateTotalCostForOrder()
    {
        var context = fixture.Context;

        var order = context.RepairOrders.First();

        var totalCost = order.Works.Sum(work => work.WorkType.Cost);

        var expectedCost = order.Works.Select(work => work.WorkType.Cost).Sum();

        Assert.Equal(expectedCost, totalCost);
        Assert.True(totalCost > 0);
    }

    /// <summary>
    /// Verifies that the five most frequently performed work types are returned in descending order.
    /// </summary>
    [Fact]
    public void ReturnTop5MostFrequentWorkTypes()
    {
        var context = fixture.Context;

        var topWorks = context.RepairOrders
            .SelectMany(order => order.Works)
            .GroupBy(orderWork => orderWork.WorkType)
            .Select(group => new
            {
                WorkType = group.Key,
                Count = group.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.Equal(5, topWorks.Count);

        Assert.All(topWorks, work => Assert.True(work.Count > 0));

        Assert.True(topWorks.SequenceEqual(topWorks.OrderByDescending(x => x.Count)));
    }

}
