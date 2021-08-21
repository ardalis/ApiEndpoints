using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace SampleEndpointApp.Authors
{
    public class Get : BaseEndpoint
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public Get(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("/authors/{id}")]
		[SwaggerOperation(
			Summary = "Get a specific Author",
			Description = "Get a specific Author",
			OperationId = "Author.Get",
			Tags = new[] { "AuthorEndpoint" })
		]
        public async Task<ActionResult<AuthorResult>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(id, cancellationToken);

            var result = _mapper.Map<AuthorResult>(author);

            return Ok(result);
        }
    }
}
