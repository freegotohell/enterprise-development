using AutoService.Domain.Data;
using AutoService.Api.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(DataSeeder.Seed());
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<MechanicService>();
builder.Services.AddScoped<WorkTypeService>();
builder.Services.AddScoped<RepairOrderService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
