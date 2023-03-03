using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using Costasdev.Geminet.Config;
using Costasdev.Geminet.Protocol;
using Microsoft.Extensions.Logging;

namespace Costasdev.Geminet;

public class Listener
{
    private readonly TcpListener _listener;
    private readonly Dictionary<string, Site> _hostsToSites;
    private readonly ILogger _logger;
    private readonly CertificateUtility _certificateUtility;

    private bool _isRunning;

    public Listener(int port, List<Site> sites, CertificateUtility certificateUtility,
        CancellationToken cancellationToken)
    {
        _hostsToSites = sites.ToDictionary(s => s.HostName);
        
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
        
        _certificateUtility = certificateUtility;
        
        _isRunning = true;
        cancellationToken.Register(() =>
        {
            _isRunning = false;
            _listener.Stop();
        });
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
                    ServerCertificate = _certificateUtility.GetCertificateForHost(info.ServerName)
                });
        }, null);

        return encryptedStream;
    }
}