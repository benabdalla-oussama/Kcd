global using Blazored.LocalStorage;
global using Kcd.UI;
global using Kcd.UI.Helpers;
global using Kcd.UI.Services.AuthService;
global using System.Net.Http.Json;
global using System.Security.Claims;
using Kcd.UI.Handlers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// Using configuration settings for HttpClient base addresses
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7268/api/v1/") });

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore(config =>
{
    config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
    config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
});
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

await builder.Build().RunAsync();
