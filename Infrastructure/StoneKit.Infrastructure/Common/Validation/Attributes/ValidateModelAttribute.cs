using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc.Filters;

namespace FluentValidation
{
    public sealed class ValidateModelAttribute : ActionFilterAttribute
    {
        public ValidateModelAttribute()
        {

        }
        private static readonly HashSet<string> ExcludeHttpVerbs = new HashSet<string> { "OPTIONS", "HEAD" };
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (ExcludeHttpVerbs.Contains(context.HttpContext.Request.Method) ||
                context.Filters.Any(filter => filter is IgnoreValidationAttribute))
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }

            object model = null;
            foreach (KeyValuePair<string, object> argument in context.ActionArguments)
            {
                if (argument.Value is IModelValidator inputModel)
                {
                    model = inputModel;
                    break;
                }
            }

            if (model == null)
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }

            Type validatorInterface = model.GetType().GetInterface(typeof(IModelValidator<>).FullName);
            if (validatorInterface == null)
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }
            Type validatorType = validatorInterface?.GetGenericArguments()[0];
            if (validatorType == null)
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }

            if (context.HttpContext.RequestServices.GetService(validatorType) is IValidator validator)
            {
                ValidationResult validationResult = validator.Validate(new ValidationContext<object>(model));
                if (!validationResult.IsValid)
                {
                    List<FluentValidationErrorModel> validationErrorModels = FluentValidationHelpers.GetFluentValidationError(validationResult.Errors);
                    string errorMessages = validationErrorModels.Select(s => s.ErrorMessage).Aggregate((a, b) => $"{a}{Environment.NewLine}{b}");

                    throw new Exception(errorMessages);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
