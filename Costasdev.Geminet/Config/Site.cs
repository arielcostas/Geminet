using System.Net;

namespace Costasdev.Geminet.Config;

public class Site
{
    public bool DefaultSite { get; }
    public string Name { get; }
    public int Port { get; }
    public string ServePath { get; }

    public Site(bool defaultSite, string name, string port, string servePath)
    {
        DefaultSite = defaultSite;
        Name = name;
        Port = int.Parse(port);
        ServePath = servePath;
    }

    public Site(string name, string port, string path) : this(false, name, port, path)
    {
    }

    public override string ToString()
    {
        return $"Site: `{Name}` Port: `{Port}` serving `{ServePath}`";
    }
}