using System.Text.Json.Serialization;

namespace Huginn.Client.Models;

public sealed class HeartbeatPayload
{
    [JsonPropertyName("serviceName")]
    public string ServiceName { get; init; } = string.Empty;

    [JsonPropertyName("instanceName")]
    public string InstanceName { get; init; } = string.Empty;

    [JsonPropertyName("hostName")]
    public string HostName { get; init; } = string.Empty;

    [JsonPropertyName("version")]
    public string Version { get; init; } = string.Empty;

    [JsonPropertyName("heartbeatTimestamp")]
    public string HeartbeatTimestamp { get; init; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; init; }

    [JsonPropertyName("environment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Environment { get; init; }
}
