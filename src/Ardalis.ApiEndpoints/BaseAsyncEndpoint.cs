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
    public abstract class BaseAsyncEndpoint<TRequest, TResponse> : BaseAsyncEndpoint
    {
        public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request);
    }

    /// <summary>
    /// A base class for an endpoint that has no parameters.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    [ApiController]
    public abstract class BaseAsyncEndpoint<TResponse> : BaseAsyncEndpoint
    {
        public abstract Task<ActionResult<TResponse>> HandleAsync();
    }
    
	/// <summary>
	/// A base class for all asynchronous endpoints.
	/// </summary>
	[ApiController]
	public abstract class BaseAsyncEndpoint : ControllerBase
	{
	}
}
