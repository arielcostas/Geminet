using Costasdev.Geminet.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Costasdev.Geminet;

public sealed class Server : BackgroundService
{
    private IConfiguration _configuration;

    public Server(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var geminetSection = _configuration.GetRequiredSection("Geminet");

        var sitesSection = geminetSection.GetRequiredSection("Sites").GetChildren();

        var sites = sitesSection.Select(s => new Site(
            s.GetValue<string>("Name") ?? throw new Exception("Site Name must be set"),
            s.GetValue<string>("HostName") ?? throw new Exception("Site HostName must be set"),
            (int)s.GetValue<int>("Listen", 1965),
            s.GetValue<string>("Serve") ?? throw new Exception("Site Serve must be set"),
            s.GetValue<bool>("GenerateIndex", false)
        )).ToList();

        var groups = sites.GroupBy(s => s.Port);
        var tasks = new List<Task>();

        var file = geminetSection.GetValue<string>("CertificatePath");
        var pass = geminetSection.GetValue<string>("CertificatePassword");

        if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(pass))
        {
            throw new Exception("CertificatePath and CertificatePassword must be set");
        }

        CertificateUtility certificateUtility = new(file, pass);

        foreach (var group in groups)
        {
            var listener = new Listener(group.Key, group.ToList(), certificateUtility, stoppingToken);
            tasks.Add(listener.Start());
        }

        return Task.WhenAll(tasks.ToArray());
    }
}