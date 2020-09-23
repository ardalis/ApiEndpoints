using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Linq;
using System.Threading.Tasks;

using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace SampleEndpointApp.Authors
{
    public class List : BaseAsyncEndpoint
    {
        [HttpGet("/authors")]
		[SwaggerOperation(
			Summary = "List all Authors",
			Description = "List all Authors",
			OperationId = "Author.List",
			Tags = new[] { "AuthorEndpoint" })
		]
        public async Task<ActionResult> HandleAsync(
            [FromServices] IAsyncRepository<Author> repository,
            [FromServices] IMapper mapper,
            [FromQuery] int page = 1, int perPage = 10,
            CancellationToken cancellationToken = default)
        {
            var result = (await repository.ListAllAsync(perPage, page, cancellationToken))
                .Select(i => mapper.Map<AuthorListResult>(i));

            return Ok(result);
        }
    }
}
