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
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
