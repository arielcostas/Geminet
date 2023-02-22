using System.Runtime.CompilerServices;
using Costasdev.Geminet.Config;

namespace Costasdev.Geminet;

public class Program
{
    static void Main(string[] args)
    {
        var sites = new Parser("server.ini").GetSites();

        // TODO: Group sites by port and host and start a server per group
        var groups = sites.GroupBy(s => s.Port);
        var tasks = new List<Task>();
        
        foreach (var group in groups)
        {
            var server = new Server(group.Key, group.ToList());
            tasks.Add(server.Start());
        }
        
        Task.WaitAll(tasks.ToArray());
    }
}