using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

public class UdpPriceClient
{
    private readonly UdpClient _client = new();
    private readonly IPEndPoint _serverEndpoint = new(IPAddress.Parse("127.0.0.1"), 8080);

    public async Task RunAsync()
    {
        Console.WriteLine("Введіть назву комплектуючої (SSD, HDD, RAM, CPU). Введіть 'exit' для виходу.");

        while (true)
        {
            Console.Write("> ");
            string? part = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(part) || part.ToLower() == "exit")
                break;

            byte[] request = Encoding.UTF8.GetBytes(part);
            await _client.SendAsync(request, request.Length, _serverEndpoint);

            var result = await _client.ReceiveAsync();
            string response = Encoding.UTF8.GetString(result.Buffer);

            Console.WriteLine($"Сервер: {response}");
        }
    }
}