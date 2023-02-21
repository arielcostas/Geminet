using Microsoft.Extensions.Configuration;

namespace Costasdev.Geminet;

public class ConfigParser
{
    public static IConfiguration LoadConfig()
    {
        return new ConfigurationBuilder()
            .AddIniFile("geminet.ini")
            .Build();
    }
}