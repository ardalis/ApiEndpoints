using Microsoft.AspNetCore.Mvc;

namespace Ardalis.ApiEndpoints
{
    [ApiController]
    public abstract class BaseEndpoint<TRequest, TResponse> : ControllerBase
    {
        public abstract ActionResult<TResponse> Handle(TRequest request);
    }

    [ApiController]
    public abstract class BaseEndpoint<TResponse> : ControllerBase
    {
        public abstract ActionResult<TResponse> Handle();
    }
}
