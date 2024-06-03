namespace System.Net
{
    public sealed class OperationResultFactoryExample : IOperationResultFactory
    {
        public OperationResult<T> CreateSuccess<T>(T? model, string? message = null)
        {
            OperationResult<T> operationResult = new OperationResult<T>
            {
                Data = model,
                ResponseInfo = new ResponseInfo
                {
                    Success = true
                }
            };

            return operationResult;
        }

        public OperationResult CreateSuccess()
        {
            return CreateSuccess<object>(null);
        }

        public void AddWarning(string message)
        {

        }

        public OperationResult<T> CreateFailure<T>(T? model, string? errorMessage, string? errorCode = null)
        {
            return new OperationResult<T>()
            {
                Data = model,
                ResponseInfo = new ResponseInfo()
                {
                    Message = errorMessage ?? "",
                    Success = false,
                    Warnings = []
                }
            };
        }
    }
}