using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ardalis.ApiEndpoints
{
    /// <summary>
    /// A base class for an endpoint that accepts parameters.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public static class BaseAsyncEndpoints
    {
        public static class WithRequest<TRequest>
        {
            [ApiController]
            public abstract class WithResponse<TResponse> : BaseAsyncEndpoint
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(
                    TRequest request,
                    CancellationToken cancellationToken = default
                );
            }

            [ApiController]
            public abstract class WithoutResponse : BaseAsyncEndpoint
            {
                public abstract Task<ActionResult> HandleAsync(
                    TRequest request,
                    CancellationToken cancellationToken = default
                );
            }
        }

        public static class WithoutRequest
        {
            [ApiController]
            public abstract class WithResponse<TResponse> : BaseAsyncEndpoint
            {
                public abstract Task<ActionResult<TResponse>> HandleAsync(
                    CancellationToken cancellationToken = default
                );
            }

            [ApiController]
            public abstract class WithoutResponse : BaseAsyncEndpoint
            {
                public abstract Task<ActionResult> HandleAsync(
                    CancellationToken cancellationToken = default
                );
            }
        }
    }

    /// <summary>
    /// A base class for all asynchronous endpoints.
    /// </summary>
    [ApiController]
    public abstract class BaseAsyncEndpoint : ControllerBase
    {
    }
}
