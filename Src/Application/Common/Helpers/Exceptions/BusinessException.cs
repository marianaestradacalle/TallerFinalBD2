using Application.Common.Utilities;
using System.Runtime.Serialization;

namespace Common.Helpers.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public BusinessException(string message, string code)
            : base(message) => Code = code;

        public string Code { get; set; }
        public static BusinessException Throw(BusinessSettings settings, string code)
        {
            ServiceException serviceExceptionDefault = new()
            {
                Id = "DefaultException",
                Code = "409.500",
                Message = "Error desconocido.",
                Description = "Este error se genera cuando no existe la propiedad en el archivo de configuración del servicio (appsettings) que contiene el listado de códigos de error."
            };
            ServiceException serviceException = settings.ServiceExceptions.FirstOrDefault(_ => _.Code.Equals(code))
                ?? serviceExceptionDefault;

            if (serviceException is null)
                throw new NullReferenceException($"No se encuentra configuración de excepciones de negocio para el tipo {code}.");

            return new(serviceException.Message, serviceException.Code);
        }


    }
}
