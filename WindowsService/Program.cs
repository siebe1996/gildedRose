using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WindowsService.ApiClient;
using WindowsService.Settings;
using WindowsService.Workers.Items;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options => options.ServiceName = "GildedTros Quality Job");

builder.Services.Configure<JobOptions>(builder.Configuration.GetSection("Job"));

builder.Services.AddSingleton<GildedRoseApiClient>(_ =>
{
    var baseUrl = Environment.GetEnvironmentVariable("services__api__https__0")
                  ?? builder.Configuration["Api:BaseUrl"]
                  ?? throw new InvalidOperationException("No API address found. Set Api:BaseUrl or run through the AppHost.");

    var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider())
    {
        BaseUrl = baseUrl
    };

    return new GildedRoseApiClient(adapter);
});

builder.Services.AddHostedService<UpdateQualityWorker>();

builder.Build().Run();