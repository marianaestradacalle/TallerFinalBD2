namespace RestApi.Health;
public class HealthResult
{
    public string ServiceName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public TimeSpan Duration { get; set; }

    public ICollection<HealthInfo>? Checks { get; set; }
}
