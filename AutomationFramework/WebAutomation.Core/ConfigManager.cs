using Microsoft.Extensions.Configuration;

namespace WebAutomation.Core.Configuration;

public static class ConfigManager
{
    private static IConfigurationRoot? _config;

    public static IConfigurationRoot Configuration =>
        _config ??= new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

    public static TestSettings Settings =>
        Configuration.GetSection("TestSettings").Get<TestSettings>()
        ?? throw new InvalidOperationException("TestSettings missing");
}