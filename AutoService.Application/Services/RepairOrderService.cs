using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace AutoService.Application.Services;

/// <summary>
/// Operations for managing repair orders
/// </summary>
public class RepairOrderService(
    IRepairOrderRepository repairOrderRepository,
    IClientRepository clientRepository,
    ICarRepository carRepository,
    IMechanicRepository mechanicRepository,
    IWorkTypeRepository workTypeRepository,
    ILogger<RepairOrderService> logger) : IRepairOrderService
{
    /// <inheritdoc />
    public async Task<List<RepairOrderDto>> GetAllAsync()
    {
        logger.LogInformation("Getting all repair orders");

        List<RepairOrder> orders = await repairOrderRepository.GetAllAsync();

        return orders.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<RepairOrderDto?> GetByIdAsync(int id)
    {
        logger.LogInformation("Getting repair order with ID {RepairOrderId}", id);

        RepairOrder? order = await repairOrderRepository.GetByIdAsync(id);

        return order is null ? null : ToDto(order);
    }

    /// <inheritdoc />
    public async Task<List<RepairOrderDto>?> GetByClientIdAsync(int clientId)
    {
        logger.LogInformation("Getting repair orders for client with ID {ClientId}", clientId);

        Client? client = await clientRepository.GetByIdAsync(clientId);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", clientId);

            return null;
        }

        List<RepairOrder> orders = await repairOrderRepository.GetByClientIdAsync(clientId);

        return orders.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<RepairOrderDto>?> GetByCarIdAsync(int carId)
    {
        logger.LogInformation("Getting repair orders for car with ID {CarId}", carId);

        Car? car = await carRepository.GetByIdAsync(carId);

        if (car is null)
        {
            logger.LogWarning("Car with ID {CarId} was not found", carId);

            return null;
        }

        List<RepairOrder> orders = await repairOrderRepository.GetByCarIdAsync(carId);

        return orders.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<RepairOrderDto>?> GetByMechanicIdAsync(int mechanicId)
    {
        logger.LogInformation("Getting repair orders for mechanic with ID {MechanicId}", mechanicId);

        Mechanic? mechanic = await mechanicRepository.GetByIdAsync(mechanicId);

        if (mechanic is null)
        {
            logger.LogWarning("Mechanic with ID {MechanicId} was not found", mechanicId);

            return null;
        }

        List<RepairOrder> orders = await repairOrderRepository.GetByMechanicIdAsync(mechanicId);

        return orders.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<List<RepairOrderDto>?> GetByWorkTypeIdAsync(int workTypeId)
    {
        logger.LogInformation("Getting repair orders for work type with ID {WorkTypeId}", workTypeId);

        WorkType? workType = await workTypeRepository.GetByIdAsync(workTypeId);

        if (workType is null)
        {
            logger.LogWarning("Work type with ID {WorkTypeId} was not found", workTypeId);

            return null;
        }

        List<RepairOrder> orders = await repairOrderRepository.GetByWorkTypeIdAsync(workTypeId);

        return orders.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<RepairOrderCostDto?> GetTotalCostAsync(int id)
    {
        logger.LogInformation("Calculating total cost for repair order with ID {RepairOrderId}", id);

        RepairOrder? order = await repairOrderRepository.GetByIdAsync(id);

        if (order is null)
        {
            logger.LogWarning("Repair order with ID {RepairOrderId} was not found", id);

            return null;
        }

        var totalCost = order.Works.Sum(work => work.WorkType.Cost);

        return new RepairOrderCostDto
        {
            RepairOrderId = order.Id,
            TotalCost = totalCost
        };
    }

    /// <inheritdoc />
    public async Task<RepairOrderDto?> CreateAsync(CreateRepairOrderDto dto)
    {
        logger.LogInformation("Creating a new repair order");

        Client? client = await clientRepository.GetByIdAsync(dto.ClientId);

        if (client is null)
        {
            logger.LogWarning("Cannot create repair order, client with ID {ClientId} was not found", dto.ClientId);

            return null;
        }

        Car? car = await carRepository.GetByIdAsync(dto.CarId);

        if (car is null || car.ClientId != client.Id)
        {
            logger.LogWarning("Cannot create repair order, car with ID {CarId} was not found or car with ID {CarId} does not belong to client with ID {ClientId}", dto.CarId, dto.CarId, dto.ClientId);

            return null;
        }

        var mechanics = (await mechanicRepository.GetAllAsync()).Where(mechanic => dto.MechanicIds.Contains(mechanic.Id)).ToList();

        if (mechanics.Count != dto.MechanicIds.Distinct().Count())
        {
            logger.LogWarning("Cannot create repair order, one or more mechanics were not found");

            return null;
        }

        var workTypes = (await workTypeRepository.GetAllAsync()).Where(workType => dto.WorkTypeIds.Contains(workType.Id)).ToList();

        if (workTypes.Count != dto.WorkTypeIds.Distinct().Count())
        {
            logger.LogWarning("Cannot create repair order, one or more work types were not found");

            return null;
        }

        List<RepairOrder> existingOrders = await repairOrderRepository.GetAllAsync();

        var nextId = existingOrders.Select(order => order.Id).DefaultIfEmpty(0).Max() + 1;

        RepairOrder order = new()
        {
            Id = nextId,
            ClientId = client.Id,
            Client = client,
            CarId = car.Id,
            Car = car,
            AdmissionDate = dto.AdmissionDate,
            ReleaseDate = dto.ReleaseDate
        };

        var nextOrderMechanicId = GetNextOrderMechanicId(existingOrders);
        var nextOrderWorkId = GetNextOrderWorkId(existingOrders);

        foreach (Mechanic mechanic in mechanics)
        {
            OrderMechanic orderMechanic = new()
            {
                Id = nextOrderMechanicId++,
                RepairOrderId = order.Id,
                RepairOrder = order,
                MechanicId = mechanic.Id,
                Mechanic = mechanic
            };

            order.Mechanics.Add(orderMechanic);
            mechanic.Orders.Add(orderMechanic);
        }

        foreach (WorkType workType in workTypes)
        {
            OrderWork orderWork = new()
            {
                Id = nextOrderWorkId++,
                RepairOrderId = order.Id,
                RepairOrder = order,
                WorkTypeId = workType.Id,
                WorkType = workType
            };

            order.Works.Add(orderWork);
            workType.Orders.Add(orderWork);
        }

        await repairOrderRepository.AddAsync(order);

        client.Orders.Add(order);
        car.Orders.Add(order);

        logger.LogInformation("Created repair order {RepairOrderId}", order.Id);

        return ToDto(order);
    }

    /// <inheritdoc />
    public async Task<RepairOrderDto?> UpdateAsync(int id, UpdateRepairOrderDto dto)
    {
        logger.LogInformation("Updating repair order with ID {RepairOrderId}", id);

        RepairOrder? order = await repairOrderRepository.GetByIdAsync(id);

        if (order is null)
        {
            logger.LogWarning("Repair order with ID {RepairOrderId} was not found", id);

            return null;
        }

        Client? client = await clientRepository.GetByIdAsync(dto.ClientId);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", dto.ClientId);

            return null;
        }

        Car? car = await carRepository.GetByIdAsync(dto.CarId);

        if (car is null || car.ClientId != client.Id)
        {
            logger.LogWarning("Cannot update repair order, car with ID {CarId} was not found or car with ID {CarId} does not belong to client with ID {ClientId}", dto.CarId, dto.CarId, dto.ClientId);

            return null;
        }

        var mechanics = (await mechanicRepository.GetAllAsync()).Where(mechanic => dto.MechanicIds.Contains(mechanic.Id)).ToList();

        if (mechanics.Count != dto.MechanicIds.Distinct().Count())
        {
            logger.LogWarning("Cannot update repair order, one or more mechanics were not found");

            return null;
        }

        var workTypes = (await workTypeRepository.GetAllAsync()).Where(workType => dto.WorkTypeIds.Contains(workType.Id)).ToList();

        if (workTypes.Count != dto.WorkTypeIds.Distinct().Count())
        {
            logger.LogWarning("Cannot update repair order, one or more work types were not found");

            return null;
        }

        order.Client.Orders.Remove(order);
        order.Car.Orders.Remove(order);

        foreach (OrderMechanic orderMechanic in order.Mechanics)
        {
            orderMechanic.Mechanic.Orders.Remove(orderMechanic);
        }

        foreach (OrderWork orderWork in order.Works)
        {
            orderWork.WorkType.Orders.Remove(orderWork);
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

        List<RepairOrder> existingOrders = await repairOrderRepository.GetAllAsync();

        var nextOrderMechanicId = GetNextOrderMechanicId(existingOrders);
        var nextOrderWorkId = GetNextOrderWorkId(existingOrders);

        foreach (Mechanic mechanic in mechanics)
        {
            OrderMechanic orderMechanic = new()
            {
                Id = nextOrderMechanicId++,
                RepairOrderId = order.Id,
                RepairOrder = order,
                MechanicId = mechanic.Id,
                Mechanic = mechanic
            };

            order.Mechanics.Add(orderMechanic);
            mechanic.Orders.Add(orderMechanic);
        }

        foreach (WorkType workType in workTypes)
        {
            OrderWork orderWork = new()
            {
                Id = nextOrderWorkId++,
                RepairOrderId = order.Id,
                RepairOrder = order,
                WorkTypeId = workType.Id,
                WorkType = workType
            };

            order.Works.Add(orderWork);
            workType.Orders.Add(orderWork);
        }

        await repairOrderRepository.UpdateAsync(order);

        logger.LogInformation("Updated repair order {RepairOrderId}", order.Id);

        return ToDto(order);
    }

    /// <inheritdoc />
    public async Task<DeleteResult> DeleteAsync(int id)
    {
        logger.LogInformation("Deleting repair order with ID {RepairOrderId}", id);

        RepairOrder? order = await repairOrderRepository.GetByIdAsync(id);

        if (order is null)
        {
            logger.LogWarning("Repair order with ID {RepairOrderId} was not found", id);

            return DeleteResult.NotFound;
        }

        order.Client.Orders.Remove(order);
        order.Car.Orders.Remove(order);

        foreach (OrderMechanic orderMechanic in order.Mechanics)
        {
            orderMechanic.Mechanic.Orders.Remove(orderMechanic);
        }

        foreach (OrderWork orderWork in order.Works)
        {
            orderWork.WorkType.Orders.Remove(orderWork);
        }

        await repairOrderRepository.DeleteAsync(order);

        logger.LogInformation("Deleted repair order {RepairOrderId}", order.Id);

        return DeleteResult.Deleted;
    }

    private static int GetNextOrderMechanicId(IEnumerable<RepairOrder> orders)
    {
        return orders
            .SelectMany(order => order.Mechanics)
            .Select(orderMechanic => orderMechanic.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;
    }

    private static int GetNextOrderWorkId(IEnumerable<RepairOrder> orders)
    {
        return orders
            .SelectMany(order => order.Works)
            .Select(orderWork => orderWork.Id)
            .DefaultIfEmpty(0)
            .Max() + 1;
    }

    private static RepairOrderDto ToDto(RepairOrder order)
    {
        return new RepairOrderDto
        {
            Id = order.Id,
            ClientId = order.ClientId,
            CarId = order.CarId,
            AdmissionDate = order.AdmissionDate,
            ReleaseDate = order.ReleaseDate,
            MechanicIds = order.Mechanics
                .Select(orderMechanic => orderMechanic.MechanicId)
                .ToList(),
            WorkTypeIds = order.Works
                .Select(orderWork => orderWork.WorkTypeId)
                .ToList()
        };
    }
}