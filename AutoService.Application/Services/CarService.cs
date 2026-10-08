using AutoService.Contracts.DTOs;
using AutoService.Contracts.Services;
using AutoService.Domain.Entities;
using AutoService.Domain.Interfaces.Repositories;
using AutoService.Domain.Shared.Results;
using Microsoft.Extensions.Logging;

namespace AutoService.Application.Services;

/// <summary>
/// Operations for managing cars
/// </summary>
public class CarService(ICarRepository carRepository, IClientRepository clientRepository, ILogger<CarService> logger) : ICarService
{
    /// <inheritdoc />
    public async Task<List<CarDto>> GetAllAsync()
    {
        logger.LogInformation("Getting all cars");

        List<Car> cars = await carRepository.GetAllAsync();

        return cars.Select(ToDto).ToList();
    }

    /// <inheritdoc />
    public async Task<CarDto?> GetByIdAsync(int id)
    {
        logger.LogInformation("Getting car with ID {CarId}", id);

        Car? car = await carRepository.GetByIdAsync(id);

        return car is null ? null : ToDto(car);
    }

    /// <inheritdoc />
    public async Task<CarDto?> CreateAsync(CreateCarDto dto)
    {
        logger.LogInformation("Creating a car for client with ID {ClientId}", dto.ClientId);

        Client? client = await clientRepository.GetByIdAsync(dto.ClientId);

        if (client is null)
        {
            logger.LogWarning("Cannot create car, client with ID {ClientId} was not found", dto.ClientId);

            return null;
        }

        List<Car> cars = await carRepository.GetAllAsync();

        var car = new Car
        {
            Id = cars.Count == 0 ? 1 : cars.Max(x => x.Id) + 1,
            LicensePlate = dto.LicensePlate,
            Brand = dto.Brand,
            Model = dto.Model,
            Year = dto.Year,
            ClientId = client.Id,
            Client = client
        };

        await carRepository.AddAsync(car);
        client.Cars.Add(car);

        logger.LogInformation("Car with ID {CarId} was created", car.Id);

        return ToDto(car);
    }

    /// <inheritdoc />
    public async Task<CarDto?> UpdateAsync(int id, UpdateCarDto dto)
    {
        logger.LogInformation("Updating car with ID {CarId}", id);

        Car? car = await carRepository.GetByIdAsync(id);

        if (car is null)
        {
            logger.LogWarning("Car with ID {CarId} was not found", id);

            return null;
        }

        Client? client = await clientRepository.GetByIdAsync(dto.ClientId);

        if (client is null)
        {
            logger.LogWarning("Cannot update car, client with ID {ClientId} was not found", dto.ClientId);

            return null;
        }

        if (car.ClientId != client.Id)
        {
            Client oldClient = car.Client;

            oldClient.Cars.Remove(car);
            client.Cars.Add(car);

            car.ClientId = client.Id;
            car.Client = client;
        }

        car.LicensePlate = dto.LicensePlate;
        car.Brand = dto.Brand;
        car.Model = dto.Model;
        car.Year = dto.Year;

        await carRepository.UpdateAsync(car);

        logger.LogInformation("Car with ID {CarId} was updated", car.Id);

        return ToDto(car);
    }

    /// <inheritdoc />
    public async Task<DeleteResult> DeleteAsync(int id)
    {
        logger.LogInformation("Deleting car with ID {CarId}", id);

        Car? car = await carRepository.GetByIdAsync(id);

        if (car is null)
        {
            logger.LogWarning("Car with ID {CarId} was not found.", id);

            return DeleteResult.NotFound;
        }

        var hasOrders = await carRepository.HasOrdersAsync(id);

        if (hasOrders)
        {
            logger.LogWarning("Cannot delete car with ID {CarId} because it has repair orders.", id);

            return DeleteResult.HasRelatedEntities;
        }

        car.Client.Cars.Remove(car);
        await carRepository.DeleteAsync(car);

        logger.LogInformation("Car with ID {CarId} was deleted", id);

        return DeleteResult.Deleted;
    }

    /// <inheritdoc />
    public async Task<List<CarDto>?> GetByClientIdAsync(int clientId)
    {
        logger.LogInformation("Getting cars for client with ID {ClientId}", clientId);

        Client? client = await clientRepository.GetByIdAsync(clientId);

        if (client is null)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", clientId);

            return null;
        }

        List<Car> cars = await carRepository.GetByClientIdAsync(clientId);

        return cars.Select(ToDto).ToList();
    }

    private static CarDto ToDto(Car car)
    {
        return new CarDto
        {
            Id = car.Id,
            LicensePlate = car.LicensePlate,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            ClientId = car.ClientId
        };
    }
}