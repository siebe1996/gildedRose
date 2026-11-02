var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Api>("api");

builder.AddProject<Projects.GildedRose_App>("app")
    .WithReference(api)
    .WaitFor(api);


builder.AddProject<Projects.WindowsService>("windowsservice")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();