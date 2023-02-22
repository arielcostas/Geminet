using Newtonsoft.Json;

namespace Costasdev.Geminet.Config;

public class Parser
{
    private readonly ConfigRoot _configRoot;

    public Parser(string configFilename)
    {
        var sr = new StreamReader(configFilename);
        ConfigRoot? cr = JsonConvert.DeserializeObject<ConfigRoot>(sr.ReadToEnd());
        if (cr == null)
        {
            throw new Exception("Invalid configuration file");
        }

        _configRoot = cr;
    }

    public List<Site> GetSites()
    {
        List<Site> sites = new();

        foreach (var site in _configRoot.sites)
        {
            sites.Add(new(
                site.name,
                site.hostname,
                site.listen,
                site.serve
            ));
        }

        return sites;
    }
}