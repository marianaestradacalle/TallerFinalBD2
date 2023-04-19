using Application.Common.Utilities;
using System.Collections.Generic;

namespace Common.Tests.Utilities.Builders;

public class AppSettingsBuilder
{
    private string _domainName;
    private string _defaultCountry;
    private string _healthChecksEndPoint;
    private ServiceException _serviceExceptionByDefault;
    private IEnumerable<ServiceException> _serviceExceptions;

    public AppSettingsBuilder()
    { }

    public AppSettingsBuilder WithDomainName(string domainName)
    {
        _domainName = domainName;
        return this;
    }
    public AppSettingsBuilder WithDefaultCountry(string defaultCountry)
    {
        _defaultCountry = defaultCountry;
        return this;
    }
    public AppSettingsBuilder WithHealthChecksEndPoint(string healthChecksEndPoint)
    {
        _healthChecksEndPoint = healthChecksEndPoint;
        return this;
    }

    public AppSettingsBuilder WithServiceExceptionByDefault(ServiceException serviceExceptionByDefault)
    {
        _serviceExceptionByDefault = serviceExceptionByDefault;
        return this;
    }

    public AppSettingsBuilder WithServiceExceptions(IEnumerable<ServiceException> serviceExceptions)
    {
        _serviceExceptions = serviceExceptions;
        return this;
    }

    public BusinessSettings Build()
    {
        return new BusinessSettings
        {
            DomainName = _domainName,
            DefaultCountry = _defaultCountry,
            HealthChecksEndPoint = _healthChecksEndPoint,
            ServiceExceptionByDefault = _serviceExceptionByDefault,
            ServiceExceptions = _serviceExceptions
        };
    }
}
