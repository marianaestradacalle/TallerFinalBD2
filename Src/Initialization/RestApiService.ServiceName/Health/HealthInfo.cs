namespace RestApi.Health;
public class HealthInfo
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TimeSpan Duration { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Error { get; set; } = string.Empty;
}
