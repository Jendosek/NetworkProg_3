using System.Net;

namespace Server;

public class ClientManager
{
    private readonly Dictionary<IPEndPoint, DateTime> _lastActivity = new();
    private readonly TimeSpan _inactiveTimeout = TimeSpan.FromMinutes(10);

    public void DisconnectInactiveClients()
    {
        var inactiveClients = new List<IPEndPoint>();

        foreach (var entry in _lastActivity)
        {
            if (DateTime.UtcNow - entry.Value > _inactiveTimeout)
            {
                inactiveClients.Add(entry.Key);
            }
        }

        foreach (var client in inactiveClients)
        {
            _lastActivity.Remove(client);
            Console.WriteLine($"Клієнт {client} був відключений через неактивність.");
        }
    }

    public void UpdateLastActivity(IPEndPoint clientEndPoint)
    {
        _lastActivity[clientEndPoint] = DateTime.UtcNow;
    }

    public int GetActiveClientsCount()
    {
        return _lastActivity.Count;
    }
}