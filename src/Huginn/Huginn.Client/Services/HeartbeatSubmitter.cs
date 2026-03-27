using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Huginn.Client.Models;

namespace Huginn.Client.Services;

public sealed class HeartbeatSubmitter
{
    private readonly HttpClient _httpClient;

    public HeartbeatSubmitter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> SubmitAsync(HuginnConfig config, HeartbeatPayload payload, CancellationToken cancellationToken)
    {
        var uri = new Uri(new Uri(config.EndpointBaseUrl.TrimEnd('/') + '/'), config.IngestPath.TrimStart('/'));
        using var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-API-Key", config.ApiKey);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return await _httpClient.SendAsync(request, cancellationToken);
    }
}
