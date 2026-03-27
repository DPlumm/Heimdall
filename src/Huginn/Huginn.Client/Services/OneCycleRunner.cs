using Huginn.Client.Logging;

namespace Huginn.Client.Services;

public static class OneCycleRunner
{
    public static async Task<int> RunAsync(string configPath, CancellationToken cancellationToken)
    {
        var config = ConfigLoader.Load(configPath);
        SafeLogger.Startup(configPath, config.EndpointBaseUrl, config.ApiKey);

        var payload = HeartbeatFactory.Build(config, DateTimeOffset.UtcNow);

        using var httpClient = new HttpClient();
        var submitter = new HeartbeatSubmitter(httpClient);
        var response = await submitter.SubmitAsync(config, payload, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            SafeLogger.SubmissionSuccess((int)response.StatusCode);
            return 0;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        SafeLogger.SubmissionFailure((int)response.StatusCode, body);
        return 1;
    }
}
