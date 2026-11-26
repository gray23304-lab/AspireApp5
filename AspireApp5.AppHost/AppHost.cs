var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddDotnetProject("server", "../AspireApp5.Server/AspireApp5.Server.csproj")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithReference(server)
    .WaitFor(server);

server.PublishWithContainerFiles(webfrontend, "wwwroot");

builder.Build().Run();
