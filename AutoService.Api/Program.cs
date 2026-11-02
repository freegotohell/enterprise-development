using AutoService.Contracts.Interfaces;
using AutoService.Infrastructure.InMemory;
using System.Text.Json.Serialization;
using AutoService.Api.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddInMemoryInfrastructure();

// Add services to the container.
builder.Services.AddScoped<IClientService, AutoService.Application.Services.ClientService>();
builder.Services.AddScoped<IMechanicService, AutoService.Application.Services.MechanicService>();
builder.Services.AddScoped<IWorkTypeService, AutoService.Application.Services.WorkTypeService>();
builder.Services.AddScoped<ICarService, AutoService.Application.Services.CarService>();
builder.Services.AddScoped<IRepairOrderService, AutoService.Application.Services.RepairOrderService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "AutoService API v1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
