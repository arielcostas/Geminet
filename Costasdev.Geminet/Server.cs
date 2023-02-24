using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Costasdev.Geminet.Config;
using Costasdev.Geminet.Protocol;

namespace Costasdev.Geminet;

public class Server
{
    private TcpListener _listener;
    private Dictionary<string, Site> _hostsToSites;
    private bool _isRunning = true;

    public Server(int port, List<Site> sites)
    {
        _hostsToSites = new Dictionary<string, Site>();
        foreach (var site in sites)
        {
            _hostsToSites.Add(site.HostName, site);
        }

        _listener = new TcpListener(IPAddress.Any, port);
    }

    public async Task Start()
    {
        _listener.Start();
        Console.WriteLine($"Listening on port {_listener.LocalEndpoint}");
        while (_isRunning)
        {
            var client = await _listener.AcceptTcpClientAsync();
            ProcessClient(client);
        }
    }

    private async void ProcessClient(TcpClient client)
    {
        var stream = await InitTls(client, null);

        var sr = new StreamReader(stream);
        var sw = new StreamWriter(stream);

        var line = await sr.ReadLineAsync();

        if (line == null)
        {
            Console.WriteLine("Invalid request");
            return;
        }

        var uri = new Uri(line);
        var site = _hostsToSites[uri.Host];

        if (site == null)
        {
            Console.WriteLine($"No site found for host {uri.Host}");
            return;
        }

        var resp = new Response($"""
# Hola

Protocolo: {uri.Scheme}
Host: {uri.Host}
Puerto: {uri.Port}
Path: {uri.AbsolutePath}
Query: {uri.Query}

""" + _hostsToSites);

        sw.WriteLine(resp);
        await sw.FlushAsync();
    }

    private async Task<SslStream> InitTls(TcpClient client, X509Certificate2 cert)
    {
        var stream = client.GetStream();
        var encryptedStream = new SslStream(stream);

        await encryptedStream.AuthenticateAsServerAsync(new SslServerAuthenticationOptions
        {
            ServerCertificate = cert,
            EnabledSslProtocols = SslProtocols.Tls12
        });

        return encryptedStream;
    }
}