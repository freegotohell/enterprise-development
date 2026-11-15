using AutoService.Domain.Entities;
using AutoService.Domain.Shared.Enums;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Infrastructure.EfCore.Data;

/// <summary>
/// Asynchronously generates and seeds test data for the auto service domain model using EF Core
/// </summary>
public static class EfCoreDataSeeder
{
    /// <summary>
    /// Seeds the database with initial test data if no clients exist
    /// </summary>
    public static async Task SeedAsync(AutoServiceDbContext context)
    {
        if (await context.Clients.AnyAsync())
        {
            return;
        }

        var workTypes = await context.WorkTypes.OrderBy(x => x.Id).ToListAsync();

        if (workTypes.Count == 0)
        {
            throw new InvalidOperationException("No work types were found");
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var clients = CreateClients();
            var mechanics = CreateMechanics();

            context.Clients.AddRange(clients);
            context.Mechanics.AddRange(mechanics);
            await context.SaveChangesAsync();

            var cars = CreateCars(clients);
            context.Cars.AddRange(cars);
            await context.SaveChangesAsync();

            var orders = CreateOrders(clients, cars, mechanics, workTypes);
            context.RepairOrders.AddRange(orders);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Creates test clients
    /// </summary>
    private static List<Client> CreateClients()
    {
        return new Faker<Client>()
            .RuleFor(x => x.FullName, f => f.Name.FullName())
            .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber())
            .Generate(10);
    }

    /// <summary>
    /// Creates test mechanics with various specializations
    /// </summary>
    private static List<Mechanic> CreateMechanics()
    {
        return new Faker<Mechanic>()
            .RuleFor(x => x.PassportNumber, f => f.Random.Replace("##########"))
            .RuleFor(x => x.FullName, f => f.Name.FullName())
            .RuleFor(
                x => x.Specialization,
                f => (MechanicSpecialization)(f.IndexFaker % Enum.GetValues<MechanicSpecialization>().Length))
            .RuleFor(x => x.Experience, f => f.Random.Int(1, 30))
            .Generate(10);
    }

    /// <summary>
    /// Creates test cars and associates them with existing clients
    /// </summary>
    private static List<Car> CreateCars(List<Client> clients)
    {
        return new Faker<Car>()
            .RuleFor(x => x.LicensePlate, f => f.Random.Replace("??####??"))
            .RuleFor(x => x.Brand, f => f.Vehicle.Manufacturer())
            .RuleFor(x => x.Model, f => f.Vehicle.Model())
            .RuleFor(x => x.Year, f => f.Random.Int(2000, 2025))
            .RuleFor(x => x.Client, f => clients[f.IndexFaker % clients.Count])
            .Generate(10);
    }

    /// <summary>
    /// Creates test repair orders with associated works and mechanics
    /// </summary>
    private static List<RepairOrder> CreateOrders(List<Client> clients, List<Car> cars, List<Mechanic> mechanics, List<WorkType> workTypes)
    {
        var orders = new List<RepairOrder>();
        var faker = new Faker();

        for (var i = 0; i < 20; i++)
        {
            var car = faker.PickRandom(cars);
            var client = clients.First(x => x.Id == car.ClientId);
            var admissionDate = faker.Date.Recent(20);

            var order = new RepairOrder
            {
                Client = client,
                Car = car,
                AdmissionDate = admissionDate,
                ReleaseDate = admissionDate.AddDays(faker.Random.Int(1, 7))
            };

            var selectedWorks = faker.PickRandom(workTypes, 3).ToList();

            foreach (var workType in selectedWorks)
            {
                order.Works.Add(new OrderWork
                {
                    RepairOrder = order,
                    WorkType = workType
                });

                var mechanic = SelectMechanic(workType.Category, mechanics, faker);

                if (order.Mechanics.All(x => x.Mechanic.Id != mechanic.Id))
                {
                    order.Mechanics.Add(new OrderMechanic
                    {
                        RepairOrder = order,
                        Mechanic = mechanic
                    });
                }
            }

            orders.Add(order);
        }

        return orders;
    }

    /// <summary>
    /// Selects a mechanic whose specialization matches the work category. General maintenance work can be assigned to any mechanic
    /// </summary>
    private static Mechanic SelectMechanic(WorkCategory category, List<Mechanic> mechanics, Faker faker)
    {
        return category switch
        {
            WorkCategory.Engine => mechanics.First(x => x.Specialization == MechanicSpecialization.Engine),
            WorkCategory.Transmission => mechanics.First(x => x.Specialization == MechanicSpecialization.Transmission),
            WorkCategory.Electrical => mechanics.First(x => x.Specialization == MechanicSpecialization.Electrical),
            WorkCategory.Diagnostics => mechanics.First(x => x.Specialization == MechanicSpecialization.Diagnostics),
            WorkCategory.BodyRepair => mechanics.First(x => x.Specialization == MechanicSpecialization.BodyRepair),
            WorkCategory.Maintenance => faker.PickRandom(mechanics),
            _ => throw new ArgumentOutOfRangeException(nameof(category), category, "Unknown work category")
        };
    }

}