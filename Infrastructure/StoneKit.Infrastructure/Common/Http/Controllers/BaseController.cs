using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using StoneKit.Infrastructure.Common;

using System.Net;

namespace Microsoft.AspNetCore.Mvc
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(Constants.JsonContentType)]
    [Produces(Constants.JsonContentType)]
    [UniqueHeader]
    [ValidateModel]
    public abstract class BaseController : ControllerBase
    {
        protected IOperationResultFactory OperationResultFactory;

        public BaseController() : base()
        {
            OperationResultFactory = HttpContext.RequestServices.GetService<IOperationResultFactory>()!;
        }


    }
}
