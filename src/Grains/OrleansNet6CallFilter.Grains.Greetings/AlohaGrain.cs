using System.Text;
using Orleans;
using OrleansNet6CallFilter.Interfaces.DemoRpcs;

namespace OrleansNet6CallFilter.Grains.Greetings;

public class AlohaGrain : Grain, IAlohaGrain
{
    public Task<string> SayAloha(int alohaCount = 1)
    {
        if (alohaCount < 1)
        {
            throw new Exception("Aloha count must be greater than 0.");
        }

        var output = new StringBuilder();
        for (var i = 0; i < alohaCount; i++)
        {
            output.AppendLine("Aloha!");
        }

        var result = output.ToString().TrimEnd('\r', '\n');
        return Task.FromResult(result);
    }
}