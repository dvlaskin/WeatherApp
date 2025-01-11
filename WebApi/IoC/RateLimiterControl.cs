using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace WebApi.IoC;

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
                    limitOptions.PermitLimit = 5;
                    limitOptions.Window = TimeSpan.FromSeconds(10);
                    limitOptions.QueueLimit = 5;
                    limitOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limitOptions.SegmentsPerWindow = 1;
                }
            );
        });

        return services;
    }
}