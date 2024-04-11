var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var apiBaseAddress = builder.Configuration["ApiSettings:GatewayAddress"];
ArgumentException.ThrowIfNullOrWhiteSpace(apiBaseAddress, "ApiSettings:GatewayAddress");

builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(config =>
    {
        config.BaseAddress = new Uri(apiBaseAddress);
    });

builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(config =>
    {
        config.BaseAddress = new Uri(apiBaseAddress);
    });

builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(config =>
    {
        config.BaseAddress = new Uri(apiBaseAddress);
    });

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

app.MapRazorPages();

app.Run();
