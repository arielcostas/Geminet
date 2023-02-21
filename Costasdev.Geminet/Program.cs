namespace ServidorGemini;

public class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
        Server server = new(1965);
        await server.Start();
    }
}