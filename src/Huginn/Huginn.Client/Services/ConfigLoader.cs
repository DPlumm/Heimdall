using System.Text.Json;
using Huginn.Client.Models;

namespace Huginn.Client.Services;

public static class ConfigLoader
{
    public static HuginnConfig Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new InvalidOperationException($"Config file not found: {path}");
        }

        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<HuginnConfig>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (config is null)
        {
            throw new InvalidOperationException("Config file could not be parsed.");
        }

        Validate(config);
        return config;
    }

    public static void Validate(HuginnConfig config)
    {
        var required = new Dictionary<string, string>
        {
            [nameof(config.EndpointBaseUrl)] = config.EndpointBaseUrl,
            [nameof(config.ApiKey)] = config.ApiKey,
            [nameof(config.ServiceName)] = config.ServiceName,
            [nameof(config.InstanceName)] = config.InstanceName,
            [nameof(config.HostName)] = config.HostName,
            [nameof(config.Version)] = config.Version,
            [nameof(config.Status)] = config.Status
        };

        var missing = required.Where(kvp => string.IsNullOrWhiteSpace(kvp.Value)).Select(kvp => kvp.Key).ToArray();
        if (missing.Length > 0)
        {
            throw new InvalidOperationException($"Missing required configuration values: {string.Join(", ", missing)}");
        }

        var allowedStatus = new[] { "Healthy", "Degraded", "Unhealthy" };
        if (!allowedStatus.Contains(config.Status))
        {
            throw new InvalidOperationException($"Unsupported status '{config.Status}'. Allowed values: {string.Join(", ", allowedStatus)}");
        }
    }
}
