using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace WebApi.IoC;

public static class OpenTelemetryControl
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var useOtltExporter = !string.IsNullOrEmpty(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
        if (useOtltExporter)
        {
            services.AddOpenTelemetry()
                // .WithMetrics(metrics =>
                // {
                //     metrics.AddRuntimeInstrumentation()
                //         .AddMeter(
                //             "Microsoft.AspNetCore.Hosting",
                //             "Microsoft.AspNetCore.Server.Kestrel",
                //             "System.Net.Http"
                //         );
                // })
                .WithTracing(tracing =>
                {
                    tracing
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation();
                }).UseOtlpExporter();
        }
        
        return services;
    }
}