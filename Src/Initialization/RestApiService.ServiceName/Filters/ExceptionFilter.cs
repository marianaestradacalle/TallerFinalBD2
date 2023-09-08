using Common.Helpers.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestApi.Filters;
public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<ExceptionFilter> _logger;


    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;

        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(BusinessException), HandleBusinessException }, 
            { typeof(CommonExceptions), HandleCommonException }
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
        else 
        {
            var details = new
            {
                Function = context.HttpContext.Request.Path.Value,
                ErrorCode =StatusCodes.Status500InternalServerError,
                Message = context.Exception.Message,
                Country = "co",
                Data = ""
            };

            context.Result = new ObjectResult(details);
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;

            return ;
          
        }
    }

    private void HandleBusinessException(ExceptionContext context)
    {
        _logger.LogError(context.Exception.ToString());

        var exception = (BusinessException)context.Exception;

        var details = new
        {
            Function = context.HttpContext.Request.Path.Value,
            ErrorCode = exception.Code,
            Message = $"Business Exception - {exception.Message}",
            Country = "co",
            Data = ""
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleCommonException(ExceptionContext context)
    {
        _logger.LogError(context.Exception.ToString());

        var exception = (CommonExceptions)context.Exception;

        var details = new
        {
            Function = context.HttpContext.Request.Path.Value,
            ErrorCode = exception.Code,
            Message = $"Common Exception - {exception.Message}",
            Country = "co",
            Data = ""
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
}
