using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace RestApiService.ServiceName.Validations
{
    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            var details = new
            {
                Function = context.HttpContext.Request.Path.Value,
                ErrorCode = 400,
                Message = ErrorMessagesHandle(context),
                Country = "co",
                Data = string.Empty

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
