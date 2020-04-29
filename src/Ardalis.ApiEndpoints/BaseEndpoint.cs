using Microsoft.AspNetCore.Mvc;

namespace Ardalis.ApiEndpoints
{
    /// <summary>
    /// A base class for an endpoint that accepts parameters.
    /// </summary>
    /// <typeparam name="TRequest">The type to be model bound by the Handle method.</typeparam>
    /// <typeparam name="TResponse">The type to return from the Handle method's ActionResult<T>.</typeparam>
    [ApiController]
    public abstract class BaseEndpoint<TRequest, TResponse> : BaseEndpoint
    {
        public abstract ActionResult<TResponse> Handle(TRequest request);
    }

    /// <summary>
    /// A base class for an endpoint that has no parameters.
    /// </summary>
    /// <typeparam name="TResponse">The type to return from the Handle method's ActionResult<T>.</typeparam>
    [ApiController]
	public abstract class BaseEndpoint<TResponse> : BaseEndpoint
    {
		public abstract ActionResult<TResponse> Handle();
    }

    /// <summary>
    /// A base class for all synchronous endpoints.
    /// </summary>
	[ApiController]
	public abstract class BaseEndpoint : ControllerBase
	{
	}
}
