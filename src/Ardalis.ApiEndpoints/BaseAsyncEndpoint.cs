using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ardalis.ApiEndpoints
{
    /// <summary>
    /// A base class for an endpoint that accepts parameters.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    [ApiController]
    public abstract class BaseAsyncEndpoint<TRequest, TResponse> : ControllerBase
    {
        public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request);
    }

    /// <summary>
    /// A base class for an endpoint that has no parameters.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    [ApiController]
    public abstract class BaseAsyncEndpoint<TResponse> : ControllerBase
    {
        public abstract Task<ActionResult<TResponse>> HandleAsync();
    }
}
