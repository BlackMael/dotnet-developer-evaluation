using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

builder.AddProject<Projects.OpenPoly_API_Service>("apiservice")
    .WithExternalHttpEndpoints()
    .WithReference(cache);

builder.Build().Run();
