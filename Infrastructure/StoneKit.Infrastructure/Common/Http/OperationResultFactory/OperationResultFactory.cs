namespace System.Net
{
    public sealed class OperationResultFactory : IOperationResultFactory
    {
        private OperationResult<object>? _operationResult;

        private List<string>? _warnings;
        public List<string> Warnings => _warnings ?? (_warnings = new List<string>());

        public OperationResult<T> CreateSuccess<T>(T? model, string? message = null)
        {
            var result = new OperationResult<T>();

            return result.Success(model, message, OperationStatusCodes.OK, _warnings);
        }

        public OperationResult CreateSuccess()
        {
            return CreateSuccess<object>(null);
        }

        public void AddWarning(string message)
        {
            Warnings.Add(message);
        }

        public OperationResult<T> CreateFailure<T>(T? model, string? errorMessage, OperationStatusCodes? errorCode = null)
        {
            var result = new OperationResult<T>();

            return result.Failure(errorMessage);
        }
    }
}