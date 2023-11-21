using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Orleans;

namespace OrleansNet6CallFilter.Client.Console.CallFilters;

// ReSharper disable once ClassNeverInstantiated.Global
public class MeasureRpcTimeCallFilter : IOutgoingGrainCallFilter
{
    private readonly ILogger<MeasureRpcTimeCallFilter> _logger;

    public MeasureRpcTimeCallFilter(ILogger<MeasureRpcTimeCallFilter> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(IOutgoingGrainCallContext context)
    {
        var excludeType = $"{context.Grain.GetType()}";
        if (excludeType is
            "Orleans.Runtime.MembershipService.MembershipTableSystemTarget" or
            "Orleans.Runtime.DeploymentLoadPublisher" or
            "Orleans.Runtime.OrleansCodeGenClusterTypeManagerReference")
        {
            // don't measure Orleans Runtime internal RPC invocations
            await context.Invoke();
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        try
        {
            await context.Invoke();
            stopwatch.Stop();
            _logger.LogInformation(
                string.Format("Call Grain {0} method {1}({2}) took {3}ms",
                    context.Grain.GetType().FullName,
                    context.InterfaceMethod.Name,
                    context.Arguments == null
                        ? string.Empty
                        : string.Join(", ", context.Arguments),
                    stopwatch.ElapsedMilliseconds));
        }
        catch (Exception)
        {
            stopwatch.Stop();
            _logger.LogError(
                string.Format("Call Grain {0} method {1}({2}) failed, took {3}ms",
                    context.Grain.GetType().FullName,
                    context.InterfaceMethod.Name,
                    context.Arguments == null
                        ? string.Empty
                        : string.Join(", ", context.Arguments),
                    stopwatch.ElapsedMilliseconds));
            throw;
        }
    }
}