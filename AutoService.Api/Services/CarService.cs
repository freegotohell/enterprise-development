using AutoService.Contracts.DTOs;
using AutoService.Domain.Data;
using AutoService.Domain.Entities;

namespace AutoService.Api.Services;

/// <summary>
/// Operations for managing cars
/// </summary>
public class CarService(AutoServiceContext context, ILogger<CarService> logger)
{
    /// <summary>
    /// Gets all cars
    /// </summary>
    public List<CarDto> GetAll()
    {
        logger.LogInformation("Getting all cars");

        return context.Cars.Select(ToDto).ToList();
    }

    /// <summary>
    /// Gets a car by id
    /// </summary>
    public CarDto? GetById(int id)
    {
        logger.LogInformation("Getting car with ID {CarId}", id);

        var car = context.Cars.FirstOrDefault(x => x.Id == id);

        return car is null ? null : ToDto(car);
    }

    /// <summary>
    /// Creates a new car
    /// </summary>
    public CarDto? Create(CreateCarDto dto)
    {
        logger.LogInformation("Creating a car for client with ID {ClientId}", dto.ClientId);

        var client = context.Clients.FirstOrDefault(x => x.Id == dto.ClientId);

        if (client is null)
        {
            logger.LogWarning("Cannot create car, client with ID {ClientId} was not found", dto.ClientId);

            return null;
        }

        var car = new Car
        {
            Id = context.Cars.Count == 0 ? 1 : context.Cars.Max(x => x.Id) + 1,
            LicensePlate = dto.LicensePlate,
            Brand = dto.Brand,
            Model = dto.Model,
            Year = dto.Year,
            ClientId = client.Id,
            Client = client
        };

        context.Cars.Add(car);
        client.Cars.Add(car);

        logger.LogInformation("Car with ID {CarId} was created", car.Id);

        return ToDto(car);
    }

    /// <summary>
    /// Updates an existing car
    /// </summary>
    public CarDto? Update(int id, UpdateCarDto dto)
    {
        logger.LogInformation("Updating car with ID {CarId}", id);

        var car = context.Cars.FirstOrDefault(x => x.Id == id);

        if (car is null)
        {
            logger.LogWarning("Car with ID {CarId} was not found", id);

            return null;
        }

        var client = context.Clients.FirstOrDefault(x => x.Id == dto.ClientId);

        if (client is null)
        {
            logger.LogWarning("Cannot update car, client with ID {ClientId} was not found", dto.ClientId);

            return null;
        }

        if (car.ClientId != client.Id)
        {
            var oldClient = car.Client;

            oldClient?.Cars.Remove(car);
            client.Cars.Add(car);

            car.ClientId = client.Id;
            car.Client = client;
        }

        car.LicensePlate = dto.LicensePlate;
        car.Brand = dto.Brand;
        car.Model = dto.Model;
        car.Year = dto.Year;

        logger.LogInformation("Car with ID {CarId} was updated", car.Id);

        return ToDto(car);
    }

    /// <summary>
    /// Deletes a car by id
    /// </summary>
    public bool Delete(int id)
    {
        logger.LogInformation("Deleting car with ID {CarId}", id);

        var car = context.Cars.FirstOrDefault(x => x.Id == id);

        if (car is null)
        {
            logger.LogWarning("Car with ID {CarId} was not found.", id);

            return false;
        }

        if (car.Orders.Count > 0)
        {
            logger.LogWarning("Cannot delete car with ID {CarId} because it has repair orders.", id);

            return false;
        }

        car.Client?.Cars.Remove(car);
        context.Cars.Remove(car);

        logger.LogInformation("Car with ID {CarId} was deleted", id);

        return true;
    }

    /// <summary>
    /// Gets a car by the client id
    /// </summary>
    public List<CarDto>? GetByClientId(int clientId)
    {
        logger.LogInformation( "Getting cars for client with ID {ClientId}", clientId);

        var clientExists = context.Clients.Any(x => x.Id == clientId);

        if (!clientExists)
        {
            logger.LogWarning("Client with ID {ClientId} was not found", clientId);

            return null;
        }

        return context.Cars.Where(x => x.ClientId == clientId).Select(ToDto).ToList();
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