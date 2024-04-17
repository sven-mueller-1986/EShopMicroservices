using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EShopMicroservices.BuildingBlocks.Logging.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request: {Request} - Response: {Response} - Request Data: {RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = Stopwatch.StartNew();

        var response = await next();

        timer.Stop();

        if (timer.Elapsed.Seconds > 3)
        {
            logger.LogWarning("[PERFORMANCE] The request {Request} took {Time} ms.",
                typeof(TRequest).Name, timer.Elapsed.Milliseconds);
        }

        logger.LogInformation("[END] Handled {Request} with {Response} in {Time} ms.",
            typeof(TRequest).Name, typeof(TResponse).Name, timer.Elapsed.Milliseconds);

        return response;
    }
}

