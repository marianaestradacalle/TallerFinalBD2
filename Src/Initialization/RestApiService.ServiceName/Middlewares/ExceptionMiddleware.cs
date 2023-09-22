using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace RestApi.Middlewares;
public static class ExceptionMiddleware
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    string errorMessage = "Milddleware Controlled Exception.";
                    string message = contextFeature.Error.GetType().Name.Equals("SqlException") ? contextFeature.Error.Source : contextFeature.Error.Message;
                    logger.LogError($"{errorMessage}. {message} {context.Request.Path}", contextFeature);
                    var details = new
                    {
                        Function = context.Request.Path.Value,
                        ErrorCode = 500,
                        Message = "An error occurred while processing your request.",
                        Country = "co",
                        Data = string.Empty
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(details));
                }
            });
        });
    }
}