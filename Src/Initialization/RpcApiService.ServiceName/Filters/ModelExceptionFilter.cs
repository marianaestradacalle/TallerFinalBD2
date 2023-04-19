using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace RpcApi.Filters;
public class ModelExceptionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {   

            var details = new
            {
                Title = "BadRequest",
                Error = ErrorMessagesHandle(context)
            };

            context.Result = new BadRequestObjectResult(details);
            return;
        }

        await next();
    }


    private string ErrorMessagesHandle(ActionContext context)
    {
        string detalle = string.Empty;
        foreach (var (index, keyModelStatePair) in context.ModelState)
        {
            if (!string.IsNullOrEmpty(keyModelStatePair.Errors[0].ErrorMessage))
                detalle += $"{keyModelStatePair.Errors[0].ErrorMessage} ";
        }
        return detalle;
    }
}
