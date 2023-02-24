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
        var certificateUtility = new CertificateUtility(_configRoot);
        List<Site> sites = new();

        foreach (var site in _configRoot.Sites!)
        {
            sites.Add(new(
                site.Name,
                site.Hostname,
                site.Listen,
                site.Serve,
                certificateUtility.GetCertificateForHost(site.Hostname)
            ));
        }

        return sites;
    }
}