using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using OrleansNet6CallFilter.Interfaces.DemoRpcs;

namespace OrleansNet6CallFilter.Grains;

// ReSharper disable once InconsistentNaming
public class Grain_3 : Grain, IGrainC, IIncomingGrainCallFilter
{
    public Task<string> DemoRpc()
    {
        return Task.FromResult($"{RequestContext.Get("Call_stack")}");
    }

    public Task Invoke(IIncomingGrainCallContext context)
    {
        var callStackStr = RequestContext.Get("Call_stack") as string ?? string.Empty;
        RequestContext.Set("Call_stack",
            string.IsNullOrEmpty(callStackStr) ? IdentityString : $"{callStackStr}\r\n \u2193 \r\n{IdentityString}");

        return context.Invoke();
    }
}