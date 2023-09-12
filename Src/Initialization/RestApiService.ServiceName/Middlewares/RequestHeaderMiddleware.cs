using Common.Helpers.Exceptions;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Extensions;
using DefaultHeaders = RestApi.Middlewares.Enums.DefaultHeaders;

namespace RestApi.Middlewares;
public class RequestHeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environment;

    public RequestHeaderMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public async Task Invoke(HttpContext httpContext)
    {
        if (httpContext is null)
            return;

        if (httpContext.Request.Path.Value.Contains("api"))
        {
            httpContext.Request.Headers.TryGetValue(DefaultHeaders.SCOrigen.GetDisplayName().Split("|")[0], out StringValues ambienteCliente);

            List<string> errors = new();

            if (!ambienteCliente.ToArray().Contains(_environment.EnvironmentName))
                errors.Add(CommonExceptionTypes.WrongEnviromentValue.GetDisplayName());

            List<DefaultHeaders> requiredHeaders = Enum.GetValues(typeof(DefaultHeaders)).Cast<DefaultHeaders>()
                .Where(x => x != DefaultHeaders.SCOrigen)
                .ToList();

            foreach (var header in requiredHeaders)
            {
                string[] headerDescription = header.GetDisplayName().Split("|");
                httpContext.Request.Headers.TryGetValue(headerDescription[0], out StringValues headerValue);
                if (!headerValue.ToArray().Any())
                    errors.Add(string.Format(CommonExceptionTypes.HeaderIsRequired.GetDisplayName(), headerDescription[0]));
            }

            if (errors.Any())
            {
                Exception ex = new Exception(string.Join("\n ", errors));
               
                var details = new
                {
                    Function = httpContext.Request.Path,
                    ErrorCode = ex.Source,
                    Message = ex.Message,
                    Country = "co",
                    Data = string.Empty
                };

                return;
            }
        }

        httpContext.Response.OnStarting(state =>
        {
            httpContext.Response.Headers.Add(DefaultHeaders.SCOrigen.ToString(), new[] { _environment.EnvironmentName });
            return Task.FromResult(0);
        }, httpContext);
        await _next(httpContext);
    }
}