using MyOrderCart.Application.Interfaces;
using MyOrderCart.Application.Options;
using MyOrderCart.Application.Services;
using MyOrderCart.BlazorUI.Components;
using MyOrderCart.BlazorUI.Services;
using MyOrderCart.Infrastructure.Persistence;
using MyOrderCart.Infrastructure.Repositories;
using MyOrderCart.Infrastructure.Services;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IExternalOrderSender, ExternalOrderSender>();
builder.Services.AddScoped<IProductService, ProductApiService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.Configure<OrderOptions>(
	builder.Configuration.GetSection(OrderOptions.SectionName));
builder.Services.Configure<ProductApiOptions>(
	builder.Configuration.GetSection(ProductApiOptions.SectionName));
builder.Services.AddDbContext<OrderDbContext>(options =>
	options.UseInMemoryDatabase("OrderDb"));


var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
