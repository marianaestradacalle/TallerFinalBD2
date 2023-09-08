using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using RestApi.Middlewares.Enums;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RestApi.Middlewares;
public class AddRequiredHeaders : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            return;

        List<DefaultHeaders> requiredHeaders = Enum.GetValues(typeof(DefaultHeaders))
            .Cast<DefaultHeaders>()
            .ToList();

        foreach (var requiredHeader in requiredHeaders)
        {
            string[] headerDescription = requiredHeader.GetDisplayName().Split("|");
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = headerDescription[0],
                Description = headerDescription[1],
                In = ParameterLocation.Header,
                Required = true
            });
        }
    }
}