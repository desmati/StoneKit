namespace System.Net
{
    public sealed class OperationResultFactory : IOperationResultFactory
    {
        private OperationResult? _operationResult;

        private List<string>? _warnings;
        public List<string> Warnings => _warnings ?? (_warnings = new List<string>());

        public OperationResult<T> CreateSuccess<T>(T? model, string? message = null)
        {
            _operationResult = new OperationResult<T>
            {
                Data = model,
                ResponseInfo = new ResponseInfo
                {
                    Success = true,
                    Message = message ?? "",
                    Warnings = _warnings ?? new List<string>()
                }
            };

            return (OperationResult<T>)_operationResult;
        }

        public OperationResult CreateSuccess()
        {
            return CreateSuccess<object>(null);
        }

        public void AddWarning(string message)
        {
            Warnings.Add(message);
        }

        public OperationResult<T> CreateFailure<T>(T? model, string? errorMessage, string? errorCode = null)
        {
            _operationResult = new OperationResult<T>
            {
                Data = model,
                ResponseInfo = new ResponseInfo
                {
                    Success = false,
                    Message = (string.IsNullOrEmpty(errorCode) ? "" : $"{errorCode}: ") + errorMessage,
                    Warnings = _warnings ?? new List<string>(),
                    ErrorCode = errorCode ?? ""
                }
            };

            return (OperationResult<T>)_operationResult;
        }
    }
}