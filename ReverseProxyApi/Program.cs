using ReverseProxyApi;
using ReverseProxyApi.IoC;

Console.WriteLine("Staring Reverse Proxy Server...");

var builder = WebApplication.CreateBuilder(args);

// add rate limiter
builder.Services.AddApiRateLimiter();

builder.Services
    .AddServiceDiscovery()
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

builder.Services.AddAppOpenTelemetry(builder.Configuration);

var app = builder.Build();

// app.UseHttpsRedirection();
app.UseCors(builderConfig => builderConfig.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseRateLimiter();

app.MapReverseProxy().RequireRateLimiting(AppConstants.SlidingWindowLimiter);

app.Run();