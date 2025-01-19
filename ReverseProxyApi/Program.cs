using ReverseProxyApi;
using ReverseProxyApi.IoC;

Console.WriteLine("Staring Reverse Proxy Server...");

var builder = WebApplication.CreateBuilder(args);

// add rate limiter
builder.Services.AddApiRateLimiter();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// app.UseHttpsRedirection();

app.UseRateLimiter();

app.MapReverseProxy().RequireRateLimiting(AppConstants.SlidingWindowLimiter);

app.Run();