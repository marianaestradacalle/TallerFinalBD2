namespace Application.Common.Utilities;
public class BusinessSettings
{
    public string DomainName { get; set; }
    public string DefaultCountry { get; set; }
    public string CRON_SONDA_JOB { get; set; }
    public string NotificationsSendMailEvent { get; set; }
    public string Requestqueue { get; set; }
    public string Requesttopic { get; set; }
    public string HealthChecksEndPoint { get; set; }
    public string HttpPort { get; set; }
    public string GRPCPort { get; set; }
    public ServiceException ServiceExceptionByDefault { get; set; }
    public IEnumerable<ServiceException> ServiceExceptions { get; set; }
}
