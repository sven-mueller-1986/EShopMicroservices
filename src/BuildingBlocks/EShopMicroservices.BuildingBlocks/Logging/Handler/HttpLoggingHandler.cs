using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace EShopMicroservices.BuildingBlocks.Logging.Handler;

public class HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var timer = Stopwatch.StartNew();
            logger.LogInformation("Sending {Method} Request to {Uri}", request.Method, request.RequestUri);

            var response = await base.SendAsync(request, cancellationToken);

            timer.Stop();
            if (response.IsSuccessStatusCode)
                logger.LogInformation("Received {StatusCode} success response from {Uri} in {Time}ms", response.StatusCode, response.RequestMessage?.RequestUri, timer.ElapsedMilliseconds);
            else
                logger.LogWarning("Received {StatusCode} non-success response from {Uri} in {Time}ms", response.StatusCode, response.RequestMessage?.RequestUri, timer.ElapsedMilliseconds);

            return response;
        }
        catch (HttpRequestException ex)
            when (ex.InnerException is SocketException se && se.SocketErrorCode == SocketError.ConnectionRefused)
        {
            var hostWithPort = request.RequestUri?.IsDefaultPort ?? false
                ? request.RequestUri?.DnsSafeHost
                : $"{request.RequestUri?.DnsSafeHost}:{request.RequestUri?.Port}";

            logger.LogCritical(ex, "Unable to connect to {Host}. Please check the configuration to ensure the correct URL for the service is configured.", hostWithPort);

            return new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                RequestMessage = request
            };
        }
    }
}
