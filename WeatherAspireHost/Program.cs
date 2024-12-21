using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiProj = builder
    .AddProject<WebApi>(nameof(Projects.WebApi))
    .WithReference(cache);

builder.Build().Run();