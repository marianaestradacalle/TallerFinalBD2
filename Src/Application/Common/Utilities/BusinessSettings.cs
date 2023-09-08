namespace Application.Common.Utilities;
public class BusinessSettings
{
    public string DomainName { get; set; } = string.Empty;
    public string DefaultCountry { get; set; } = string.Empty;
    public string CRON_SONDA_JOB { get; set; } = string.Empty;
    public string NotificationsSendMailEvent { get; set; } = string.Empty;
    public string Requestqueue { get; set; } = string.Empty;
    public string Requesttopic { get; set; } = string.Empty;
    public string HealthChecksEndPoint { get; set; } = string.Empty;
    public string HttpPort { get; set; } = string.Empty;
    public string GRPCPort { get; set; } = string.Empty;
    public IEnumerable<ServiceException>? ServiceExceptions { get; set; }
}
