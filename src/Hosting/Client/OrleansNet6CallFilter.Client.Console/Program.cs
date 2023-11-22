using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using OrleansNet6CallFilter.Client.Console.CallFilters;
using OrleansNet6CallFilter.Interfaces.DemoRpcs;
using static System.Console;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "filter-demo";
        options.ServiceId = "Demo Orleans Call Filter";
    })
    .ConfigureApplicationParts(parts =>
    {
        parts.AddApplicationPart(typeof(IHelloGrain).Assembly).WithReferences();
        parts.AddApplicationPart(typeof(IAlohaGrain).Assembly).WithReferences();
    })
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
    .AddOutgoingGrainCallFilter<MeasureRpcTimeCallFilter>()
    .Build();

WriteLine(
    "\r\n---\r\nPlease wait until Orleans Server is started and ready for connections, then press any key to start connect...\r\n---\r\n");
ReadKey();
await client.Connect();

WriteLine("\r\n---\r\nOrleans Client connected, press any key to start Silo level Call Filter demo...\r\n---");
ReadKey();

var helloGrain = client.GetGrain<IHelloGrain>(0);
var greetingMsg = "Call Filter Demo";
var helloResult = await helloGrain.SayHello(greetingMsg);
WriteLine($"\r\n---\r\nCall HelloGrain.SayHello(\"{greetingMsg}\") =\r\n{helloResult}\r\n---");

var alohaGrain01 = client.GetGrain<IAlohaGrain>("An Aloha Grain");
var alohaGrain02 = client.GetGrain<IAlohaGrain>("Another Aloha Grain");

var alohaResult = await alohaGrain01.SayAloha();
WriteLine($"\r\n---\r\nCall An Aloha Grain.SayAloha() =\r\n{alohaResult}\r\n---\r\n");

try
{
    await alohaGrain01.SayAloha(-1);
}
catch (Exception ex)
{
    WriteLine($"\r\n---\r\nCall An Aloha Grain.SayAloha(-1) got exception:\r\n{ex}\r\n---");
}

var anotherAlohaResult = await alohaGrain02.SayAloha(3);
WriteLine($"\r\n---\r\nCall Another Aloha Grain.SayAloha(3) =\r\n{anotherAlohaResult}\r\n---");

WriteLine("\r\n---\r\nPress any key to run Grain level Call Filter Demo...\r\n---");
ReadKey();

var grainA = client.GetGrain<IGrainA>(Guid.NewGuid());
var callStackStr = await grainA.DemoRpc();
WriteLine($"\r\n---\r\nCall GrainA.DemoRpc() =\r\n{callStackStr}\r\n---");

WriteLine("\r\n---\r\nDemonstration finished, press any key to exit...\r\n---");
ReadKey();

await client.Close();
client.Dispose();