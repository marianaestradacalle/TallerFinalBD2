using Common.Helpers.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace RpcApi.Filters;
public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<ExceptionFilter> _logger;


    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;

        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(BusinessException), HandleBusinessException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }
    }

    private void HandleBusinessException(ExceptionContext context)
    {
        _logger.LogError(context.Exception.ToString());

        var exception = (BusinessException)context.Exception;

        var details = new
        {
            Title = "Business Exception",
            Error = exception.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
}
