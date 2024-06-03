using FluentValidation.Results;

namespace FluentValidation
{
    public static class FluentValidationHelpers
    {
        public static List<FluentValidationErrorModel> GetFluentValidationError(IList<ValidationFailure> list)
        {
            return list.Select(validationFailure => new FluentValidationErrorModel
            {
                ErrorMessage = validationFailure.ErrorMessage,
                AttemptedValue = validationFailure.AttemptedValue,
                CustomState = validationFailure.CustomState,
                ErrorCode = validationFailure.ErrorCode,
                FormattedMessagePlaceholderValues = validationFailure.FormattedMessagePlaceholderValues,
                PropertyName = validationFailure.PropertyName
            }).ToList();
        }
    }
}