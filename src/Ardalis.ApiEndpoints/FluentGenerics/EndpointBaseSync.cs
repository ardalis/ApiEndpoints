using Microsoft.AspNetCore.Mvc;

namespace Ardalis.ApiEndpoints
{
    /// <summary>
    /// A base class for an endpoint that accepts parameters.
    /// </summary>
    public static class EndpointBaseSync
    {
        public static class WithRequest<TRequest>
        {
            public abstract class WithResult<TResponse> : EndpointBase
            {
                public abstract TResponse Handle(TRequest request);
            }

            public abstract class WithoutResult: EndpointBase
            {
                public abstract void Handle(TRequest request);
            }

            public abstract class WithActionResult<TResponse> : EndpointBase
            {
                public abstract ActionResult<TResponse> Handle(TRequest request);
            }

            public abstract class WithActionResult : EndpointBase
            {
                public abstract ActionResult Handle(TRequest request);
            }
        }

        public static class WithoutRequest
        {
            public abstract class WithResult<TResponse> : EndpointBase
            {
                public abstract TResponse Handle();
            }

            public abstract class WithoutResult : EndpointBase
            {
                public abstract void Handle();
            }

            public abstract class WithActionResult<TResponse> : EndpointBase
            {
                public abstract ActionResult<TResponse> Handle();
            }

            public abstract class WithActionResult : EndpointBase
            {
                public abstract ActionResult Handle();
            }
        }
    }
}
