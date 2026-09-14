using AutoService.Tests.Fixtures;
using Xunit;

namespace AutoService.Tests;

/// <summary>
/// Contains tests for the seeded AutoService domain model.
/// </summary>
public class DomainTests(AutoServiceFixture fixture)
    : IClassFixture<AutoServiceFixture>
{
    /// <summary>
    /// Verifies that the data seeder creates the required number of domain entities.
    /// </summary>
    [Fact]
    public void SeederShouldCreateEnoughData()
    {
        var context = fixture.Context;

        Assert.True(context.Clients.Count >= 10);
        Assert.True(context.Cars.Count >= 10);
        Assert.True(context.Mechanics.Count >= 10);
        Assert.True(context.WorkTypes.Count >= 10);
        Assert.True(context.RepairOrders.Count >= 10);
    }

    /// <summary>
    /// Verifies that every seeded car is associated with a client.
    /// </summary>
    [Fact]
    public void SeederShouldCreateCarsWithClients()
    {
        var context = fixture.Context;

        var carsWithoutClient = context.Cars.Count(car => car.Client == null);

        Assert.Equal(0, carsWithoutClient);
    }

    /// <summary>
    /// Verifies that every seeded repair order has an associated car and client.
    /// </summary>
    [Fact]
    public void SeederShouldCreateRepairOrdersWithRelations()
    {
        var context = fixture.Context;

        var invalidOrders = context.RepairOrders
            .Count(order => order.Car == null || order.Client == null);

        Assert.Equal(0, invalidOrders);
    }

    /// <summary>
    /// Verifies that every seeded repair order has at least one mechanic.
    /// </summary>
    [Fact]
    public void RepairOrdersShouldHaveMechanics()
    {
        var context = fixture.Context;

        var ordersWithoutMechanics = context.RepairOrders
            .Count(order => order.Mechanics.Count == 0);

        Assert.Equal(0, ordersWithoutMechanics);
    }

    /// <summary>
    /// Verifies that every seeded repair order contains at least one work item.
    /// </summary>
    [Fact]
    public void RepairOrdersShouldHaveWorks()
    {
        var context = fixture.Context;

        var ordersWithoutWorks = context.RepairOrders
            .Count(order => order.Works.Count == 0);

        Assert.Equal(0, ordersWithoutWorks);
    }
}