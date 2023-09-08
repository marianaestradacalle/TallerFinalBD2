using Application.Common.Utilities;
using System.Collections.Generic;

namespace Dobles.Tests.Application.Common.Tests.Utilities;
public class AppSettingsBuilder
{
    private string _domainName;
    private string _defaultCountry;
    private string _healthChecksEndPoint;
    private IEnumerable<ServiceException> _serviceExceptions;

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
            ServiceExceptions = _serviceExceptions
        };
    }
}
