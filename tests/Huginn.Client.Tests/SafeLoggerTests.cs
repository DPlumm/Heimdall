using Huginn.Client.Logging;

namespace Huginn.Client.Tests;

public sealed class SafeLoggerTests
{
    [Fact]
    public void ApiKeyFingerprint_DoesNotReturnRawApiKey()
    {
        const string apiKey = "my-super-secret-key";

        var fingerprint = SafeLogger.ApiKeyFingerprint(apiKey);

        Assert.DoesNotContain(apiKey, fingerprint, StringComparison.Ordinal);
        Assert.Equal(12, fingerprint.Length);
    }
}
