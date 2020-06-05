using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Threading.Tasks;

using Swashbuckle.AspNetCore.Annotations;

namespace SampleEndpointApp.Authors
{
    public class Get : BaseAsyncEndpoint<int, AuthorResult>
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
        public override async Task<ActionResult<AuthorResult>> HandleAsync(int id)
        {
            var author = await _repository.GetByIdAsync(id);

            var result = _mapper.Map<AuthorResult>(author);

            return Ok(result);
        }
    }
}