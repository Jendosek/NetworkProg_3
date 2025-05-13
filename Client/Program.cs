namespace Client;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new UdpPriceClient();
        await client.RunAsync();
    }
}