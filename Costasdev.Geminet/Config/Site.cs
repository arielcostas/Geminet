using System.Security.Cryptography.X509Certificates;

namespace Costasdev.Geminet.Config;

public class Site
{
    public string Name { get; }
    public string HostName { get; }
    public int Port { get; }
    public string ServePath { get; }
    public bool GenerateIndex { get; }

    public Site(string name, string hostname, int port, string servePath, bool generateIndex = false)
    {
        Name = name;
        HostName = hostname;
        Port = port;
        ServePath = servePath;
        GenerateIndex = generateIndex;
    }

    public override string ToString()
    {
        return
            $"Name: {Name}, HostName: {HostName}, Port: {Port}, ServePath: {ServePath} GenerateIndex: {GenerateIndex}";
    }
}