using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using OrleansNet6CallFilter.Grains;
using OrleansNet6CallFilter.Silo.WorkerService.CallFilters;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddLogging(logBuilder =>
        {
            logBuilder.ClearProviders();
            logBuilder.AddConsole();
            if (System.Diagnostics.Debugger.IsAttached)
            {
                logBuilder.AddDebug();
            }
        });
    })
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering()
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "filter-demo";
                options.ServiceId = "Demo Orleans Call Filter";
            })
            .ConfigureApplicationParts(parts =>
            {
                parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences();
                parts.AddApplicationPart(typeof(AlohaGrain).Assembly).WithReferences();
            })
            .AddIncomingGrainCallFilter<LoggingCallFilter>();
    })
    .Build();

await host.RunAsync();