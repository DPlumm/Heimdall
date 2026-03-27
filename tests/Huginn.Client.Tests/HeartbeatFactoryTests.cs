using Huginn.Client.Models;
using Huginn.Client.Services;

namespace Huginn.Client.Tests;

public sealed class HeartbeatFactoryTests
{
    [Fact]
    public void Build_Produces_RequiredCanonicalFields()
    {
        var config = new HuginnConfig
        {
            EndpointBaseUrl = "https://localhost:5001",
            ApiKey = "test-key",
            ServiceName = "svc-a",
            InstanceName = "inst-a",
            HostName = "host-a",
            Version = "1.2.3",
            Status = "Healthy"
        };

        var payload = HeartbeatFactory.Build(config, new DateTimeOffset(2026, 3, 27, 12, 0, 0, TimeSpan.Zero));

        Assert.Equal("svc-a", payload.ServiceName);
        Assert.Equal("inst-a", payload.InstanceName);
        Assert.Equal("host-a", payload.HostName);
        Assert.Equal("1.2.3", payload.Version);
        Assert.Equal("Healthy", payload.Status);
        Assert.Equal("2026-03-27T12:00:00.0000000Z", payload.HeartbeatTimestamp);
    }
}
