using Application.Common.Utilities;

namespace Common.Helpers.Exceptions;
public class CommonExceptions : Exception
{
    private CommonExceptions(string message, string code) : base(message) => Code = code;

    public string Code { get; set; }

    public static CommonExceptions Throw(CommonExceptionTypes exceptionType, BusinessSettings settings)
    {
        ServiceException serviceExceptionDefault = new () 
        {
              Id= "DefaultException",
              Code= "409.500",
              Message= "Error desconocido.",
              Description= "Este error se genera cuando no existe la propiedad en el archivo de configuración del servicio (appsettings) que contiene el listado de códigos de error."
        };

        ServiceException serviceException = settings.ServiceExceptions.FirstOrDefault(_ => _.Id.Equals(exceptionType.ToString()))
            ?? serviceExceptionDefault;

        if (serviceException is null)
            throw new NullReferenceException($"No se encuentra configuración de excepciones de negocio para el tipo {exceptionType}.");

        return new(serviceException.Message, serviceException.Code);
    }
}
