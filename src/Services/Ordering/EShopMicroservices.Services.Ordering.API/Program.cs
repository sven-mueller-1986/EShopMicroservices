using EShopMicroservices.Services.Ordering.API;
using EShopMicroservices.Services.Ordering.Application;
using EShopMicroservices.Services.Ordering.Infrastructure;
using EShopMicroservices.Services.Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddApiServices()
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseApiServices();

if(app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();