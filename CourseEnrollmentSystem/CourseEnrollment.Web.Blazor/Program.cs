using CourseEnrollment.Web.Blazor;
using CourseEnrollment.Web.Blazor.Auth;
using CourseEnrollment.Web.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiBaseUrl is not configured.");

// Auth services (singleton token store, scoped everything else)
builder.Services.AddSingleton<AccessTokenStore>();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// Auth HTTP client — browser manages the HttpOnly cookie automatically
builder.Services.AddHttpClient("AuthClient",
    client => client.BaseAddress = new Uri(apiBaseUrl));

builder.Services.AddScoped<AuthService>(sp =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();
    var http = factory.CreateClient("AuthClient");
    var stateProvider = sp.GetRequiredService<JwtAuthenticationStateProvider>();
    return new AuthService(http, stateProvider);
});

// CourseApiService — routed through the Bearer-token handler
builder.Services.AddHttpClient<CourseApiService>(
    client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>();

await builder.Build().RunAsync();
