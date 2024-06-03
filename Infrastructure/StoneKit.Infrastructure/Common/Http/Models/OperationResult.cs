using Microsoft.AspNetCore.Mvc;

namespace System.Net
{
    public sealed class OperationResult<T> : OperationResult
    {
        public T? Data { get; set; }
    }

    public class OperationResult
    {
        public ResponseInfo? ResponseInfo { get; set; }

        public static implicit operator JsonResult(OperationResult operationResult)
        {
            return new JsonResult(operationResult);
        }

        public static implicit operator ActionResult(OperationResult operationResult)
        {
            return new JsonResult(operationResult);
        }
    }
}
