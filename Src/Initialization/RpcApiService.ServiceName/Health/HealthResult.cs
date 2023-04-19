namespace RpcApi.Health;
public class HealthResult
{
    public string ServiceName { get; set; }

    public string Status { get; set; }

    public TimeSpan Duration { get; set; }

    public ICollection<HealthInfo> Checks { get; set; }
}
