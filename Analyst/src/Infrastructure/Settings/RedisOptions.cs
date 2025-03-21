namespace Infrastructure.Settings;

public class RedisOptions
{
    public string ConnectionString { get; set; } 
    public int ConnectRetry { get; set; } 
    public int ConnectTimeout { get; set; } 
    public bool AbortOnConnectFail { get; set; } 
}