using Huginn.Client.Models;

namespace Huginn.Client.Services;

public static class HeartbeatFactory
{
    public static HeartbeatPayload Build(HuginnConfig config, DateTimeOffset timestamp)
    {
        return new HeartbeatPayload
        {
            ServiceName = config.ServiceName,
            InstanceName = config.InstanceName,
            HostName = config.HostName,
            Version = config.Version,
            HeartbeatTimestamp = timestamp.UtcDateTime.ToString("O"),
            Status = config.Status,
            Message = config.Message,
            Environment = config.Environment
        };
    }
}
