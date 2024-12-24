using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var webapi = builder
    .AddProject<WebApi>("webapi")
    .WithReference(cache)
    .WaitFor(cache)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("angular", "../WeatherFrontend")
    .WithReference(webapi)
    .WaitFor(webapi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();