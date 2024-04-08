using EShopMicroservices.Services.Discount.Grpc.Data;
using EShopMicroservices.Services.Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddSqlite<DiscountContext>(builder.Configuration.GetConnectionString("Database"));

var app = builder.Build();

// Configure the HTTP request pipeline.

// Automatically initialize DB
app.UseMigration();

app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
