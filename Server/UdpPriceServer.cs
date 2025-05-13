using System.Net.Sockets;
using System.Text;

namespace Server;

public class UdpPriceServer
{
    private readonly UdpClient _server = new(8080);

    public async Task StartAsync()
    {
        Console.WriteLine("Сервер запущено на порту 8080...");

        while (true)
        {
            var result = await _server.ReceiveAsync();
            string request = Encoding.UTF8.GetString(result.Buffer);
            Console.WriteLine($"Запит: \"{request}\" від {result.RemoteEndPoint}");

            string response = PriceDatabase.GetPrice(request);
            byte[] data = Encoding.UTF8.GetBytes(response);

            await _server.SendAsync(data, data.Length, result.RemoteEndPoint);
        }
    }
}