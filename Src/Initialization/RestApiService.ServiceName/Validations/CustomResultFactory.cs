using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using System.Net;

namespace RestApiService.ServiceName.Validations
{
    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            var details = new
            {
                Title = "BadRequest",
                Error = ErrorMessagesHandle(context)
            };

            return new BadRequestObjectResult(details);
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
}
