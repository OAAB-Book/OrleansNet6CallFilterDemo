using Orleans;
using Orleans.Runtime;

namespace OrleansNet6CallFilter.Silo.WorkerService.CallFilters;

public class LoggingCallFilter : IIncomingGrainCallFilter
{
    private readonly ILogger<LoggingCallFilter> _logger;

    public LoggingCallFilter(ILogger<LoggingCallFilter> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(IIncomingGrainCallContext context)
    {
        try
        {
            await context.Invoke();

            var methodSignature = $"{context.Grain.GetType()}";
            if (methodSignature is
                "Orleans.Runtime.MembershipService.MembershipTableSystemTarget" or
                "Orleans.Runtime.DeploymentLoadPublisher" or
                "Orleans.Runtime.OrleansCodeGenClusterTypeManagerReference")
            {
                // don't log Orleans Runtime internal RPC invocations
            }
            else
            {
                _logger.LogInformation("{0}.{1}({2}) returned value {3}",
                    context.Grain.GetType(),
                    context.InterfaceMethod.Name,
                    context.Arguments == null
                        ? string.Empty
                        : string.Join(", ", context.Arguments),
                    context.Result);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError("call {0}.{1}({2}) failed, exception:\r\n{3}",
                context.Grain.GetType(),
                context.InterfaceMethod.Name,
                context.Arguments == null
                    ? string.Empty
                    : string.Join(", ", context.Arguments),
                exception);
            // If this exception is not re-thrown, it is considered to be
            // handled by this filter.
            throw;
        }
    }
}