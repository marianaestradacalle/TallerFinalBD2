using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestApi.Filters;
public class SuccessFilter: ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        var result = (ObjectResult)context.Result;
        if (result.StatusCode.Equals(200))
        {
            var objectResult = context.Result as ObjectResult;
            var resultValue = objectResult?.Value;

            var details = new
            {
                Function = context.HttpContext.Request.Path.Value,
                ErrorCode = context.HttpContext.Response.StatusCode,
                Message = string.Empty,
                Country = "co",
                Data = resultValue
            };

            context.Result = new ObjectResult(details);
            base.OnResultExecuting(context);
        }
    }

}
