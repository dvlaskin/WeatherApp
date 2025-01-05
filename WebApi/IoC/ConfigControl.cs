namespace WebApi.IoC;

public static class ConfigControl
{
    public static IConfigurationManager AddConfig(this IConfigurationManager configManager)
    {
        configManager
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
        
        return configManager;
    }
}