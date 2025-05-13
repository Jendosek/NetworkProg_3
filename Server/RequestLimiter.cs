namespace Server;

public class RequestLimiter
{
    private int _requestCount = 0;
    private DateTime _startTime = DateTime.UtcNow;

    public bool IsAllowed()
    {
        if (DateTime.UtcNow - _startTime > TimeSpan.FromHours(1))
        {
            _startTime = DateTime.UtcNow;
            _requestCount = 0;
        }

        if (_requestCount >= 10)
            return false;

        _requestCount++;
        return true;
    }
}