namespace Huginn.Client.Models;

public sealed class HuginnConfig
{
    public string EndpointBaseUrl { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
    public string ServiceName { get; init; } = string.Empty;
    public string InstanceName { get; init; } = string.Empty;
    public string HostName { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Status { get; init; } = "Healthy";
    public string? Message { get; init; }
    public string? Environment { get; init; }
    public string IngestPath { get; init; } = "/api/v1/heartbeats";
}
