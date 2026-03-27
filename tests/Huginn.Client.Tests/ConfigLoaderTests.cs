using Huginn.Client.Models;
using Huginn.Client.Services;

namespace Huginn.Client.Tests;

public sealed class ConfigLoaderTests
{
    [Fact]
    public void Validate_Throws_When_RequiredValueMissing()
    {
        var config = new HuginnConfig
        {
            EndpointBaseUrl = "",
            ApiKey = "key",
            ServiceName = "svc",
            InstanceName = "inst",
            HostName = "host",
            Version = "1.0.0",
            Status = "Healthy"
        };

        var act = () => ConfigLoader.Validate(config);

        var ex = Assert.Throws<InvalidOperationException>(act);
        Assert.Contains("EndpointBaseUrl", ex.Message);
    }
}
