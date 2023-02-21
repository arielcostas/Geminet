using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Costasdev.Geminet.Protocol;

namespace Costasdev.Geminet;

public class Server
{
    private TcpListener _listener;

    public Server(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
    }

    //Resharper disable once FunctionNeverReturns
    public async Task Start()
    {
        _listener.Start();
        for (;;)
        {
            var client = await _listener.AcceptTcpClientAsync();
            ProcessClient(client);
        }
    }

    private async void ProcessClient(TcpClient client)
    {
        var stream = client.GetStream();
        var encryptedStream = new SslStream(stream);

        await encryptedStream.AuthenticateAsServerAsync(new SslServerAuthenticationOptions
        {
            ServerCertificate = new X509Certificate2("certs.pfx", "1234")
        });

        var sr = new StreamReader(encryptedStream);
        var sw = new StreamWriter(encryptedStream);

        var line = sr.ReadLine();
        var resp = new Response($"Hola, mandaste {line}");

        sw.WriteLine(resp);
        await sw.FlushAsync();
    }
}