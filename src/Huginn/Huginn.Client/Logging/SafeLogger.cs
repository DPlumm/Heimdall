using System.Security.Cryptography;
using System.Text;

namespace Huginn.Client.Logging;

public static class SafeLogger
{
    public static string ApiKeyFingerprint(string apiKey)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(apiKey));
        return Convert.ToHexString(bytes)[..12];
    }

    public static void Startup(string configPath, string endpointBaseUrl, string apiKey)
    {
        Console.WriteLine($"[startup] config={configPath} endpoint={endpointBaseUrl} apiKeyFingerprint={ApiKeyFingerprint(apiKey)}");
    }

    public static void SubmissionSuccess(int statusCode)
    {
        Console.WriteLine($"[submit] result=success statusCode={statusCode}");
    }

    public static void SubmissionFailure(int statusCode, string body)
    {
        Console.WriteLine($"[submit] result=failure statusCode={statusCode} body={body}");
    }
}
