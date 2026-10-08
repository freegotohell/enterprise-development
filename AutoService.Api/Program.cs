using AutoService.Contracts.Services;
using AutoService.Domain.Data;
using AutoService.Infrastructure.InMemory;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

AutoServiceContext context = DataSeeder.Seed();

builder.Services.AddSingleton(context);
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
