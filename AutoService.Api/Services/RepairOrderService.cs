using AutoService.Contracts.DTOs;
using AutoService.Domain.Data;
using AutoService.Domain.Entities;
using AutoService.Domain.Shared.Results;

namespace AutoService.Api.Services;

/// <summary>
/// Operations for managing repair orders
/// </summary>
public class RepairOrderService(AutoServiceContext context, ILogger<RepairOrderService> logger)
{
    /// <summary>
    /// Gets all repair orders
    /// </summary
    public List<RepairOrderDto> GetAll()
    {
        logger.LogInformation("Getting all repair orders");

        return context.RepairOrders.Select(ToDto).ToList();
    }

    /// <summary>
    /// Gets a repair order by id
    /// </summary>
    public RepairOrderDto? GetById(int id)
    {
        logger.LogInformation("Getting repair order with ID {RepairOrderId}", id);

        var order = context.RepairOrders.FirstOrDefault(x => x.Id == id);

        return order is null ? null : ToDto(order);
    }

    /// <summary>
    /// Gets the next available ID for an order-mechanic relationship
    /// </summary>
    private int GetNextOrderMechanicId()
    {
        return context.RepairOrders
            .SelectMany(x => x.Mechanics)
            .Select(x => x.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;
    }

    /// <summary>
    /// Gets the next available ID for an order-work relationship
    /// </summary>
    private int GetNextOrderWorkId()
    {
        return context.RepairOrders
            .SelectMany(x => x.Works)
            .Select(x => x.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;
    }

    /// <summary>
    /// Gets all repair orders placed by the client
    /// </summary>
    public List<RepairOrderDto>? GetByClientId(int clientId)
    {
        logger.LogInformation("Getting repair orders for client with ID {ClientId}", clientId);

        var clientExists = context.Clients.Any(x => x.Id == clientId);

        if (!clientExists)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", clientId);

            return null;
        }

        return context.RepairOrders.Where(x => x.ClientId == clientId).Select(ToDto).ToList();
    }

    /// <summary>
    /// Gets all repair orders by car id
    /// </summary>
    public List<RepairOrderDto>? GetByCarId(int carId)
    {
        logger.LogInformation("Getting repair orders for car with ID {CarId}", carId);

        var carExists = context.Cars.Any(x => x.Id == carId);

        if (!carExists)
        {
            logger.LogWarning("Car with ID {CarId} was not found", carId);

            return null;
        }

        return context.RepairOrders.Where(x => x.CarId == carId).Select(ToDto).ToList();
    }

    /// <summary>
    /// Gets repair orders associated with the mechanic
    /// </summary>
    public List<RepairOrderDto>? GetByMechanicId(int mechanicId)
    {
        logger.LogInformation("Getting repair orders for mechanic with ID {MechanicId}", mechanicId);

        var mechanicExists = context.Mechanics.Any(x => x.Id == mechanicId);

        if (!mechanicExists)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found", mechanicId);

            return null;
        }

        return context.RepairOrders.Where(x => x.Mechanics.Any(y => y.MechanicId == mechanicId)).Select(ToDto).ToList();
    }

    /// <summary>
    /// Gets repair orders with the work type
    /// </summary>
    public List<RepairOrderDto>? GetByWorkTypeId(int workTypeId)
    {
        logger.LogInformation("Getting repair orders for work type with ID {WorkTypeId}", workTypeId);

        var workTypeExists = context.WorkTypes.Any(x => x.Id == workTypeId);

        if (!workTypeExists)
        {
            logger.LogWarning("Work type with ID {WorkTypeId} was not found", workTypeId);

            return null;
        }

        return context.RepairOrders.Where(x => x.Works.Any(y => y.WorkTypeId == workTypeId)).Select(ToDto).ToList();
    }

    /// <summary>
    /// Creates a new repair order
    /// </summary>
    public RepairOrderDto? Create(CreateRepairOrderDto dto)
    {
        logger.LogInformation("Creating a new repair order");

        var client = context.Clients.FirstOrDefault(x => x.Id == dto.ClientId);

        if (client is null)
        {
            logger.LogWarning("Cannot create repair order, client with ID {ClientId} was not found", dto.ClientId);

            return null;
        }

        var car = context.Cars.FirstOrDefault(x => x.Id == dto.CarId);

        if (car is null)
        {
            logger.LogWarning("Cannot create repair order, car with ID {CarId} was not found", dto.CarId);

            return null;
        }

        var mechanics = context.Mechanics.Where(x => dto.MechanicIds.Contains(x.Id)).ToList();

        if (mechanics.Count != dto.MechanicIds.Distinct().Count())
        {
            logger.LogWarning("Cannot create repair order, one or more mechanics were not found");

            return null;
        }

        var workTypes = context.WorkTypes.Where(x => dto.WorkTypeIds.Contains(x.Id)).ToList();

        if (workTypes.Count != dto.WorkTypeIds.Distinct().Count())
        {
            logger.LogWarning("Cannot create repair order, one or more work types were not found");

            return null;
        }

        var order = new RepairOrder
        {
            Id = context.RepairOrders.Count == 0 ? 1 : context.RepairOrders.Max(x => x.Id) + 1,

            ClientId = client.Id,
            Client = client,

            CarId = car.Id,
            Car = car,

            AdmissionDate = dto.AdmissionDate,
            ReleaseDate = dto.ReleaseDate
        };

        context.RepairOrders.Add(order);

        client.Orders.Add(order);
        car.Orders.Add(order);

        foreach (var mechanic in mechanics)
        {
            var orderMechanic = new OrderMechanic
            {
                Id = GetNextOrderMechanicId(),
                RepairOrderId = order.Id,
                RepairOrder = order,
                MechanicId = mechanic.Id,
                Mechanic = mechanic
            };

            order.Mechanics.Add(orderMechanic);
            mechanic.Orders.Add(orderMechanic);
        }

        foreach (var workType in workTypes)
        {
            var orderWork = new OrderWork
            {
                Id = GetNextOrderWorkId(),
                RepairOrderId = order.Id,
                RepairOrder = order,
                WorkTypeId = workType.Id,
                WorkType = workType
            };

            order.Works.Add(orderWork);
            workType.Orders.Add(orderWork);
        }

        logger.LogInformation("Repair order with ID {RepairOrderId} was created", order.Id);

        return ToDto(order);
    }

    /// <summary>
    /// Updates an existing repair order
    /// </summary>
    public RepairOrderDto? Update(int id, UpdateRepairOrderDto dto)
    {
        logger.LogInformation("Updating repair order with ID {RepairOrderId}", id);

        var order = context.RepairOrders.FirstOrDefault(x => x.Id == id);

        if (order is null)
        {
            logger.LogWarning("Repair order with ID {RepairOrderId} was not found", id);

            return null;
        }

        var client = context.Clients.FirstOrDefault(x => x.Id == dto.ClientId);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", dto.ClientId);

            return null;
        }

        var car = context.Cars
            .FirstOrDefault(x => x.Id == dto.CarId);

        if (car is null)
        {
            logger.LogWarning("Car with ID {CarId} was not found", dto.CarId);

            return null;
        }

        var mechanics = context.Mechanics.Where(x => dto.MechanicIds.Contains(x.Id)).ToList();

        if (mechanics.Count != dto.MechanicIds.Distinct().Count())
        {
            logger.LogWarning("Cannot update repair order, one or more mechanics were not found");

            return null;
        }

        var workTypes = context.WorkTypes.Where(x => dto.WorkTypeIds.Contains(x.Id)).ToList();

        if (workTypes.Count != dto.WorkTypeIds.Distinct().Count())
        {
            logger.LogWarning("Cannot update repair order, one or more work types were not found");

            return null;
        }

        order.Client?.Orders.Remove(order);
        order.Car?.Orders.Remove(order);

        foreach (var orderMechanic in order.Mechanics)
        {
            orderMechanic.Mechanic?.Orders.Remove(orderMechanic);
        }

        foreach (var orderWork in order.Works)
        {
            orderWork.WorkType?.Orders.Remove(orderWork);
        }

        order.Mechanics.Clear();
        order.Works.Clear();

        order.ClientId = client.Id;
        order.Client = client;

        order.CarId = car.Id;
        order.Car = car;

        order.AdmissionDate = dto.AdmissionDate;
        order.ReleaseDate = dto.ReleaseDate;

        client.Orders.Add(order);
        car.Orders.Add(order);

        foreach (var mechanic in mechanics)
        {
            var orderMechanic = new OrderMechanic
            {
                Id = GetNextOrderMechanicId(),
                RepairOrderId = order.Id,
                RepairOrder = order,
                MechanicId = mechanic.Id,
                Mechanic = mechanic
            };

            order.Mechanics.Add(orderMechanic);
            mechanic.Orders.Add(orderMechanic);
        }

        foreach (var workType in workTypes)
        {
            var orderWork = new OrderWork
            {
                Id = GetNextOrderWorkId(),
                RepairOrderId = order.Id,
                RepairOrder = order,
                WorkTypeId = workType.Id,
                WorkType = workType
            };

            order.Works.Add(orderWork);
            workType.Orders.Add(orderWork);
        }

        logger.LogInformation("Repair order with ID {RepairOrderId} was updated", order.Id);

        return ToDto(order);
    }

    /// <summary>
    /// Deletes a repair order by id
    /// </summary>
    public DeleteResult Delete(int id)
    {
        logger.LogInformation("Deleting repair order with ID {RepairOrderId}", id);

        var order = context.RepairOrders.FirstOrDefault(x => x.Id == id);

        if (order is null)
        {
            logger.LogWarning("Repair order with ID {RepairOrderId} was not found", id);

            return DeleteResult.NotFound;
        }

        order.Client?.Orders.Remove(order);
        order.Car?.Orders.Remove(order);

        foreach (var orderMechanic in order.Mechanics)
        {
            orderMechanic.Mechanic?.Orders.Remove(orderMechanic);
        }

        foreach (var orderWork in order.Works)
        {
            orderWork.WorkType?.Orders.Remove(orderWork);
        }

        context.RepairOrders.Remove(order);

        logger.LogInformation("Repair order with ID {RepairOrderId} was deleted", id);

        return DeleteResult.Deleted;
    }
    /// <summary>
    /// Maps a RepairOrder entity to a RepairOrderDto
    /// </summary>
    private static RepairOrderDto ToDto(RepairOrder order)
    {
        return new RepairOrderDto
        {
            Id = order.Id,
            ClientId = order.ClientId,
            CarId = order.CarId,
            AdmissionDate = order.AdmissionDate,
            ReleaseDate = order.ReleaseDate,
            MechanicIds = order.Mechanics.Select(x => x.MechanicId).ToList(),
            WorkTypeIds = order.Works.Select(x => x.WorkTypeId).ToList()
        };
    }
}