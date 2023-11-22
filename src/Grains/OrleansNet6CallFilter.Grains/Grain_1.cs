using Orleans;
using Orleans.Runtime;
using OrleansNet6CallFilter.Interfaces.DemoRpcs;

namespace OrleansNet6CallFilter.Grains;

// ReSharper disable once InconsistentNaming
public class Grain_1 : Grain, IGrainA, IIncomingGrainCallFilter
{
    public Task<string> DemoRpc()
    {
        var grain2 = GrainFactory.GetGrain<IGrainB>(Guid.NewGuid());
        return grain2.DemoRpc();
    }

    public Task Invoke(IIncomingGrainCallContext context)
    {
        var callStackStr = RequestContext.Get("Call_stack") as string ?? string.Empty;
        RequestContext.Set("Call_stack",
            string.IsNullOrEmpty(callStackStr) ? IdentityString : $"{callStackStr}\r\n \u2193 \r\n{IdentityString}");

        return context.Invoke();
    }
}