using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder
    .AddRedis("cache")
    .WithDataVolume();

var webapi = builder
    .AddProject<WebApi>("webapi")
    .WithReference(cache)
    .WithExternalHttpEndpoints();

var apiReverseProxy = builder
    .AddProject<ReverseProxyApi>("apiReverseProxy")
    .WithReference(webapi)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("angular", "../WeatherFrontend")
    .WithReference(apiReverseProxy)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();