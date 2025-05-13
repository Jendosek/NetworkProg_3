namespace Server;

class Program
{
    static async Task Main(string[] args)
    {
        var server = new UdpPriceServer();
        await server.StartAsync();
    }
}