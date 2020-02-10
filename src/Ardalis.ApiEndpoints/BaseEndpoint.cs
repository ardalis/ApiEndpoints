using Microsoft.AspNetCore.Mvc;

namespace Ardalis.ApiEndpoints
{
    /// <summary>
    /// A base class for an endpoint that accepts parameters.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    [ApiController]
    public abstract class BaseEndpoint<TRequest, TResponse> : ControllerBase
    {
        public abstract ActionResult<TResponse> Handle(TRequest request);
    }

    /// <summary>
    /// A base calss for an endpoint that has no parameters.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    [ApiController]
    public abstract class BaseEndpoint<TResponse> : ControllerBase
    {
        public abstract ActionResult<TResponse> Handle();
    }
}
