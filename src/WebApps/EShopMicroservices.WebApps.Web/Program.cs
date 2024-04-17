using Serilog;
using EShopMicroservices.BuildingBlocks.Logging;
using EShopMicroservices.BuildingBlocks.Logging.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddSeriLogger();

builder.Services.AddRazorPages();

var apiBaseAddress = builder.Configuration["ApiSettings:GatewayAddress"];
ArgumentException.ThrowIfNullOrWhiteSpace(apiBaseAddress, "ApiSettings:GatewayAddress");

builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(config =>
    {
        config.BaseAddress = new Uri(apiBaseAddress);
    })
    .AddHttpMessageHandler<HttpLoggingHandler>();

builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(config =>
    {
        config.BaseAddress = new Uri(apiBaseAddress);
    })
    .AddHttpMessageHandler<HttpLoggingHandler>();

builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(config =>
    {
        config.BaseAddress = new Uri(apiBaseAddress);
    })
    .AddHttpMessageHandler<HttpLoggingHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapRazorPages();

app.Run();
