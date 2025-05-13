using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

public class UdpPriceServer
{
    private readonly UdpClient _server = new(8080);
    private readonly Dictionary<IPEndPoint, RequestLimiter> _requestLimiters = new();

    public async Task StartAsync()
    {
        Console.WriteLine("Сервер запущено на порту 8080...");

        while (true)
        {
            var result = await _server.ReceiveAsync();
            string request = Encoding.UTF8.GetString(result.Buffer);
            IPEndPoint clientEndPoint = result.RemoteEndPoint;

            Console.WriteLine($"Запит: \"{request}\" від {clientEndPoint}");
            
            if (!IsRequestAllowed(clientEndPoint))
            {
                string limitExceededMessage = "Перевищено ліміт запитів (не більше 10 за годину)";
                byte[] limitData = Encoding.UTF8.GetBytes(limitExceededMessage);
                await _server.SendAsync(limitData, limitData.Length, clientEndPoint);
                continue;
            }

            string response = PriceDatabase.GetPrice(request);
            byte[] data = Encoding.UTF8.GetBytes(response);
            
            await _server.SendAsync(data, data.Length, clientEndPoint);
        }
    }

    private bool IsRequestAllowed(IPEndPoint clientEndPoint)
    {
        if (!_requestLimiters.ContainsKey(clientEndPoint))
        {
            _requestLimiters[clientEndPoint] = new RequestLimiter();
        }

        return _requestLimiters[clientEndPoint].IsAllowed();
    }
}