using Microsoft.Extensions.Configuration;

namespace Costasdev.Geminet.Config;

public class Parser
{
    private readonly IConfiguration _configuration;

    public Parser(string configFilename)
    {
        _configuration = new ConfigurationBuilder()
            .AddIniFile(configFilename)
            .Build();
    }

    public List<Site> GetSites()
    {
        List<Site> sites = new();

        var config = (from child in _configuration.GetChildren() select child)
            .ToDictionary(child => child.Key,
                child => child.Value);

        sites.Add(new(
            true,
            "default",
            config["port"] ?? "1965",
            config["path"] ?? Directory.GetCurrentDirectory()
        ));
        
        var nonDefaultSites = (
            from child in _configuration.GetChildren()
            where child.Value == null
            select child
        ).ToList();
        
        foreach (var site in nonDefaultSites)
        {
            sites.Add(new(
                site.Key,
                site["port"] ?? "1965",
                site["path"] ?? Directory.GetCurrentDirectory()
            ));
        }

        return sites;
    }
}