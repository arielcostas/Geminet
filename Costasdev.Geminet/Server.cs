using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using Costasdev.Geminet.Config;
using Costasdev.Geminet.Protocol;
using Microsoft.Extensions.Logging;

namespace Costasdev.Geminet;

public class Server
{
    private TcpListener _listener;
    private Dictionary<string, Site> _hostsToSites;
    private bool _isRunning = true;
    private ILogger _logger;

    public Server(int port, List<Site> sites)
    {
        _hostsToSites = new Dictionary<string, Site>();
        foreach (var site in sites)
        {
            _hostsToSites.Add(site.HostName, site);
        }

        _listener = new TcpListener(IPAddress.Any, port);
        _logger = LoggerFactory.Create(conf => conf
                .SetMinimumLevel(LogLevel.Debug)
                .AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                })
            )
            .CreateLogger("SRV-" + port);
    }

    public async Task Start()
    {
        _listener.Start();
        _logger.LogInformation("Listening");
        while (_isRunning)
        {
            var client = await _listener.AcceptTcpClientAsync();
            _logger.LogInformation("Accepted connection from {}", client.Client.RemoteEndPoint);
            ProcessClient(client);
        }
    }

    private async void ProcessClient(TcpClient client)
    {
        SslStream stream;
        try
        {
            stream = await InitTls(client);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error cifrando conexión: {}", ex.Message);
            return;
        }

        var sr = new StreamReader(stream);
        var sw = new StreamWriter(stream);

        var line = await sr.ReadLineAsync();
        _logger.LogInformation("Received request: {}", line ?? "null");

        if (line == null)
        {
            _logger.LogInformation("Invalid request");
            return;
        }

        var uri = new Uri(line);

        if (!_hostsToSites.ContainsKey(uri.Host))
        {
            _logger.LogError("No site found for host {0}", uri.Host);
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
        
        _logger.LogInformation("Sending response: {}", resp.Body.Length);

        sw.WriteLine(resp);
        await sw.FlushAsync();
        stream.Close();
        _logger.LogInformation("S'ha acabat");
    }

    private async Task<SslStream> InitTls(TcpClient client)
    {
        var stream = client.GetStream();
        var encryptedStream = new SslStream(stream);

        await encryptedStream.AuthenticateAsServerAsync((_, info, _, _) =>
        {
            if (!_hostsToSites.ContainsKey(info.ServerName))
            {
                throw new Exception("Host not found. Cannot start TLS");
            }

            return new ValueTask<SslServerAuthenticationOptions>(
                new SslServerAuthenticationOptions
                {
                    ServerCertificate = _hostsToSites[info.ServerName].Certificate
                });
        }, null);

        return encryptedStream;
    }
}