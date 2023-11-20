using Orleans;
using Orleans.Configuration;
using static System.Console;
using OrleansNet6CallFilter.Interfaces.DemoRpcs;

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
    .Build();

WriteLine(
    "Please wait until Orleans Server is started and ready for connections, then press any key to start connect...");
ReadKey();
await client.Connect();
WriteLine("\r\n---\r\nOrleans Client connected\r\n---");

var helloGrain = client.GetGrain<IHelloGrain>(0);
var greetingMsg = "Call Filter Demo";
var helloResult = await helloGrain.SayHello(greetingMsg);
WriteLine($"\r\n---\r\nCall HelloGrain.SayHello(\"{greetingMsg}\") =\r\n{helloResult}\r\n---");

var alohaGrain01 = client.GetGrain<IAlohaGrain>("An Aloha Grain");
var alohaGrain02 = client.GetGrain<IAlohaGrain>("Another Aloha Grain");

var alohaResult = await alohaGrain01.SayAloha();
WriteLine($"\r\n---\r\nCall An Aloha Grain.SayAloha() =\r\n{alohaResult}\r\n---\r\n");
var anotherAlohaResult = await alohaGrain02.SayAloha(3);
WriteLine($"\r\n---\r\nCall Another Aloha Grain.SayAloha(3) =\r\n{anotherAlohaResult}\r\n---");

WriteLine("\r\n---\r\nDemonstration finished, press any key to exit...");
ReadKey();

await client.Close();
client.Dispose();