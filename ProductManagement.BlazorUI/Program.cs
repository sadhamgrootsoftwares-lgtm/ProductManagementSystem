using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProductManagement.BlazorUI;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ProductManagement.BlazorUI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API Address
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7245") 
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<CustomAuthStateProvider>());

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ToastService>();


builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthService>();


await builder.Build().RunAsync();
