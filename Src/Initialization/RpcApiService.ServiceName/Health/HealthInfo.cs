namespace RpcApi.Health;
public class HealthInfo
{
    public string Name { get; set; }

    public string Description { get; set; }

    public TimeSpan Duration { get; set; }

    public string Status { get; set; }

    public string Error { get; set; }
}
