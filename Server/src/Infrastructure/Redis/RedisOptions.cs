namespace Infrastructure.Redis;

public class RedisOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public int ConnectRetry { get; set; } = 3;
    public int ConnectTimeout { get; set; } = 5000;
    public bool AbortOnConnectFail { get; set; } = false;
}