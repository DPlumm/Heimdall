using Huginn.Client.Services;

var configPath = args.Length >= 2 && args[0] == "--config"
    ? args[1]
    : "huginn.config.json";

var exitCode = await OneCycleRunner.RunAsync(configPath, CancellationToken.None);
return exitCode;
