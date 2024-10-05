namespace System.Net
{
    public interface IOperationResultFactory
    {
        OperationResult<T> CreateSuccess<T>(T model, string? messageKey = null);
        OperationResult<T> CreateFailure<T>(T model, string? errorMessage, OperationStatusCodes? errorCode = null);
        OperationResult CreateSuccess();

        void AddWarning(string message);
    }
}