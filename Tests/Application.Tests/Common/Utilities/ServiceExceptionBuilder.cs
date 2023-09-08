using Application.Common.Utilities;

namespace Application.Tests.Common.Utilities;
public class ServiceExceptionBuilder
{
    private string _id = "DefaultException";
    private string _code = "409.500";
    private string _message = "Error desconocido.";
    private string _description = "Este error se genera cuando no existe la propiedad en el archivo de configuración del servicio (appsettings) que contiene el listado de códigos de error.";

    public ServiceExceptionBuilder()
    { }

    public ServiceExceptionBuilder WithId(string id)
    {
        _id = id;
        return this;
    }

    public ServiceExceptionBuilder WithCode(string code)
    {
        _code = code;
        return this;
    }

    public ServiceExceptionBuilder WithMessage(string message)
    {
        _message = message;
        return this;
    }

    public ServiceExceptionBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public ServiceException Build()
    {
        return new ServiceException
        {
            Id = _id,
            Code = _code,
            Message = _message,
            Description = _description
        };
    }
}
