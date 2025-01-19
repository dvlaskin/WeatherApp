using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace ReverseProxyApi.IoC;

public static class RateLimiterControl
{
    public static IServiceCollection AddApiRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddSlidingWindowLimiter(
                AppConstants.SlidingWindowLimiter,
                limitOptions =>
                {
                    limitOptions.PermitLimit = 20;
                    limitOptions.Window = TimeSpan.FromSeconds(10);
                    limitOptions.QueueLimit = 10;
                    limitOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limitOptions.SegmentsPerWindow = 1;
                }
            );
        });

        return services;
    }
}