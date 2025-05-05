using MyOrderCart.Application.Interfaces;
using MyOrderCart.BlazorUI.Components;
using MyOrderCart.BlazorUI.Services;
using MyOrderCart.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IExternalOrderSender, ExternalOrderSender>();
builder.Services.AddScoped<IProductApiService, ProductApiService>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<CartService>();


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
