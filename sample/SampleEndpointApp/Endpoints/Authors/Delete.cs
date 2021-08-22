using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace SampleEndpointApp.Endpoints.Authors
{
    public partial class Delete : EndpointBase
    {
        private readonly IAsyncRepository<Author> _repository;

        public Delete(IAsyncRepository<Author> repository)
        {
            _repository = repository;
        }

        [HttpDelete("{id}")]
		[SwaggerOperation(
			Summary = "Deletes an Author",
			Description = "Deletes an Author",
			OperationId = "Author.Delete",
			Tags = new[] { "AuthorEndpoint" })
		]
        public async Task<ActionResult> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(id, cancellationToken);

            if (author is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(author, cancellationToken);

            // see https://restfulapi.net/http-methods/#delete
            return NoContent();
        }
    }
}
