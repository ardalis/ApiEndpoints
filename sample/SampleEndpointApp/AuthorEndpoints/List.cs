using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Swashbuckle.AspNetCore.Annotations;

namespace SampleEndpointApp.Authors
{
    public class List : BaseAsyncEndpoint
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public List(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("/authors")]
		[SwaggerOperation(
			Summary = "List all Authors",
			Description = "List all Authors",
			OperationId = "Author.List",
			Tags = new[] { "AuthorEndpoint" })
		]
        public async Task<ActionResult> HandleAsync([FromQuery] int page = 1, int perPage = 10)
        {
            var result = (await _repository.ListAllAsync(perPage, page))
                .Select(i => _mapper.Map<AuthorListResult>(i));

            return Ok(result);
        }
    }
}