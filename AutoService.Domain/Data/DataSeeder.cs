using AutoService.Domain.Entities;
using AutoService.Domain.Enums;
using Bogus;

namespace AutoService.Domain.Data;

/// <summary>
/// Generates test data for the auto service domain model.
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Creates and populates an in-memory auto service data context.
    /// </summary>
    public static AutoServiceContext Seed()
    {
        var context = new AutoServiceContext();

        CreateClients(context);
        CreateCars(context);
        CreateMechanics(context);
        CreateWorkTypes(context);
        CreateOrders(context);

        return context;
    }

    /// <summary>
    /// Creates test clients and adds them to the context.
    /// </summary>
    private static void CreateClients(AutoServiceContext context)
    {
        var clientFaker = new Faker<Client>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Phone, f => f.Phone.PhoneNumber());

        context.Clients.AddRange(clientFaker.Generate(10));
    }

    /// <summary>
    /// Creates test mechanics and adds them to the context.
    /// </summary>
    private static void CreateMechanics(AutoServiceContext context)
    {
        var mechanicFaker = new Faker<Mechanic>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.PassportNumber, f => f.Random.Replace("##########"))
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Experience, f => f.Random.Int(1, 30))
            .RuleFor(
                x => x.Specialization,
                f =>(MechanicSpecialization)(
                    f.IndexFaker % Enum.GetValues<MechanicSpecialization>().Length));

        context.Mechanics.AddRange(mechanicFaker.Generate(10));
    }

    /// <summary>
    /// Creates predefined work types and adds them to the context.
    /// </summary>
    private static void CreateWorkTypes(AutoServiceContext context)
    {
        var works = new List<WorkType>
        {
            new()
            {
                Id = 1,
                Name = "Oil And Filter Change",
                Category = WorkCategory.Maintenance,
                Cost = 2500,
                Duration = TimeSpan.FromHours(1),
                Description = "Engine Oil And Oil Filter Replacement"
            },

            new()
            {
                Id = 2,
                Name = "Engine Diagnostics",
                Category = WorkCategory.Diagnostics,
                Cost = 5000,
                Duration = TimeSpan.FromHours(2),
                Description = "Computerized Engine Diagnostic Testing"
            },

            new()
            {
                Id = 3,
                Name = "Engine Repair",
                Category = WorkCategory.Engine,
                Cost = 15000,
                Duration = TimeSpan.FromHours(6),
                Description = "Repair Of Engine Components And Systems"
            },

            new()
            {
                Id = 4,
                Name = "Clutch Replacement",
                Category = WorkCategory.Transmission,
                Cost = 20000,
                Duration = TimeSpan.FromHours(6),
                Description = "Clutch Assembly Replacement"
            },

            new()
            {
                Id = 5,
                Name = "Electrical Diagnostics",
                Category = WorkCategory.Electrical,
                Cost = 4000,
                Duration = TimeSpan.FromHours(2),
                Description = "Electrical System Diagnostic And Troubleshooting"
            },

            new()
            {
                Id = 6,
                Name = "Electrical Wiring Repair",
                Category = WorkCategory.Electrical,
                Cost = 7000,
                Duration = TimeSpan.FromHours(3),
                Description = "Repair And Restoration Of Electrical Wiring"
            },

            new()
            {
                Id = 7,
                Name = "Body Repair",
                Category = WorkCategory.BodyRepair,
                Cost = 25000,
                Duration = TimeSpan.FromHours(8),
                Description = "Repair Of Vehicle Body Damage"
            },

            new()
            {
                Id = 8,
                Name = "Routine Vehicle Maintenance",
                Category = WorkCategory.Maintenance,
                Cost = 8000,
                Duration = TimeSpan.FromHours(3),
                Description = "Scheduled Vehicle Maintenance And Inspection"
            },

            new()
            {
                Id = 9,
                Name = "Engine Overhaul",
                Category = WorkCategory.Engine,
                Cost = 50000,
                Duration = TimeSpan.FromHours(12),
                Description = "Complete Engine Rebuild And Overhaul"
            },

            new()
            {
                Id = 10,
                Name = "Transmission Diagnostics",
                Category = WorkCategory.Transmission,
                Cost = 6000,
                Duration = TimeSpan.FromHours(2),
                Description = "Transmission System Diagnostic And Inspection"
            }
        };

        context.WorkTypes.AddRange(works);
    }

    /// <summary>
    /// Creates test cars and associates them with existing clients.
    /// </summary>
    private static void CreateCars(AutoServiceContext context)
    {
        var carFaker = new Faker<Car>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(
                x => x.LicensePlate,
                f => $"{f.Random.String2(3, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")}{f.Random.Int(100, 999)}")
            .RuleFor(x => x.Brand, f => f.Vehicle.Manufacturer())
            .RuleFor(x => x.Model, f => f.Vehicle.Model())
            .RuleFor(x => x.Year, f => f.Random.Int(2000, 2026))
            .RuleFor(x => x.ClientId, f => f.PickRandom(context.Clients).Id)
            .RuleFor(x => x.Client, (f, x) => context.Clients.First(c => c.Id == x.ClientId));

        var cars = carFaker.Generate(10);

        foreach (var car in cars)
        {
            car.Client.Cars.Add(car);
        }

        context.Cars.AddRange(cars);
    }

    /// <summary>
    /// Creates test repair orders and assigns works and mechanics to them.
    /// </summary>
    private static void CreateOrders(AutoServiceContext context)
    {
        var orderFaker = new Faker<RepairOrder>()
            .RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.CarId, f => f.PickRandom(context.Cars).Id)
            .RuleFor(
                x => x.ClientId,
                (f, x) => context.Cars.First(car => car.Id == x.CarId).ClientId)
            .RuleFor(
                x => x.AdmissionDate,
                f => f.Random.Bool() ? f.Date.Recent(20) : f.Date.Past(1))
            .RuleFor(
                x => x.ReleaseDate,
                (f, order) => order.AdmissionDate.AddDays(f.Random.Int(1, 7)));


        var orders = orderFaker.Generate(20);

        foreach (var order in orders)
        {
            order.Client = context.Clients.First(x => x.Id == order.ClientId);
            order.Car =context.Cars.First(x => x.Id == order.CarId);

            order.Car.Orders.Add(order);
            order.Client.Orders.Add(order);

            AddWorks(context, order);
            AddMechanics(context, order);

        }

        context.RepairOrders.AddRange(orders);
    }


    private static int _orderMechanicId = 1;
    private static int _orderWorkId = 1;

    /// <summary>
    /// Assigns mechanics to a repair order according to the required work specializations.
    /// </summary>
    private static void AddMechanics(AutoServiceContext context, RepairOrder order)
    {
        var requiredSpecializations = order.Works
                .Select(work => work.WorkType.Category)
                .Where(category => category != WorkCategory.Maintenance)
                .Select(category => category switch
                {
                    WorkCategory.Engine => MechanicSpecialization.Engine,
                    WorkCategory.Transmission => MechanicSpecialization.Transmission,
                    WorkCategory.Electrical => MechanicSpecialization.Electrical,
                    WorkCategory.Diagnostics => MechanicSpecialization.Diagnostics,
                    WorkCategory.BodyRepair => MechanicSpecialization.BodyRepair,
                    _ => throw new InvalidOperationException(
                        $"Unsupported work category: {category}")
                })
                .Distinct()
                .ToList();

        var mechanics = context.Mechanics
                .Where(mechanic => requiredSpecializations.Contains(mechanic.Specialization))
                .GroupBy(mechanic => mechanic.Specialization)
                .Select(group => group.OrderBy(_ => Guid.NewGuid()).First())
                .ToList();

        if (order.Works.Any(work =>
                work.WorkType.Category == WorkCategory.Maintenance))
        {
            var randomMechanic = context.Mechanics.OrderBy(_ => Guid.NewGuid()).First();

            if (!mechanics.Any(mechanic => mechanic.Id == randomMechanic.Id))
            {
                mechanics.Add(randomMechanic);
            }
        }

        foreach (var mechanic in mechanics)
        {
            var orderMechanic = new OrderMechanic
            {
                Id = _orderMechanicId++,
                RepairOrder = order,
                RepairOrderId = order.Id,
                Mechanic = mechanic,
                MechanicId = mechanic.Id
            };

            order.Mechanics.Add(orderMechanic);
            mechanic.Orders.Add(orderMechanic);
        }
    }

    /// <summary>
    /// Assigns randomly selected work types to a repair order.
    /// </summary>
    private static void AddWorks(AutoServiceContext context, RepairOrder order)
    {
        var works = context.WorkTypes.OrderBy(_ => Guid.NewGuid()).Take(3).ToList();

        foreach (var work in works)
        {
            var orderWork = new OrderWork
            {
                Id = _orderWorkId++,
                RepairOrder = order,
                RepairOrderId = order.Id,
                WorkType = work,
                WorkTypeId = work.Id
            };

            order.Works.Add(orderWork);
            work.Orders.Add(orderWork);
        }
    }
}