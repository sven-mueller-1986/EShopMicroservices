using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace EShopMicroservices.WebApps.Web.Policies;

public static class RequestPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> RetryPolicy =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 5, 
                sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(500) * retryAttempt, 
                onRetry: (message, retryTime, context) => 
                {
                    Log.Error("Retry {RetryTime} of {RequestMethod} at {RequestUrl}, due to {Exception}.", retryTime, message.Result.RequestMessage?.Method, message.Result.RequestMessage?.RequestUri, message.Exception);
                });
    

    public static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy =>    
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30));
    
}
