using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

public class UdpPriceServer
{
    private readonly UdpClient _server = new(8080);
    private readonly Dictionary<IPEndPoint, RequestLimiter> _requestLimiters = new();
    private readonly ClientManager _clientManager = new();
    private readonly int _maxClients = 5;

    public async Task StartAsync()
    {
        Console.WriteLine("Сервер запущено на порту 8080...");

        while (true)
        {
            _clientManager.DisconnectInactiveClients();
            
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
            
            if (_clientManager.GetActiveClientsCount() >= _maxClients)
            {
                string clientLimitExceededMessage = "Максимальна кількість одночасно підключених клієнтів досягнута.";
                byte[] clientLimitData = Encoding.UTF8.GetBytes(clientLimitExceededMessage);
                await _server.SendAsync(clientLimitData, clientLimitData.Length, clientEndPoint);
                continue;
            }
            
            _clientManager.UpdateLastActivity(clientEndPoint);

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