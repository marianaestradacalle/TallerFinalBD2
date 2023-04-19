using Application.Common.Utilities;
using System.Runtime.Serialization;

namespace Common.Helpers.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base(message) { }

        public BusinessException(string message, string code)
            : base(message) => Code = code;

        protected BusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public string Code { get; set; }
        public static BusinessException Throw(BusinessExceptionTypes exceptionType, BusinessSettings settings)
        {
            ServiceException serviceException = settings.ServiceExceptions.FirstOrDefault(_ => _.Id.Equals(exceptionType.ToString())) ??
                settings.ServiceExceptionByDefault;

            if (serviceException is null)
                throw new NullReferenceException($"No se encuentra configuración de excepciones de negocio para el tipo {exceptionType}.");

            return new(serviceException.Message, serviceException.Code);
        }

    }
    public enum BusinessExceptionTypes
    {
        NotControlledException,
        InvalidNoteId,
        InvalidNoteListId,
        RecordNotFound
    }
}
