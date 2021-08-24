using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Endpoints.Authors
{
    public class Get : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult<AuthorResult>
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public Get(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a specific Author
        /// </summary>
        [HttpGet("/authors/{id}")]
        public override async Task<ActionResult<AuthorResult>> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(id, cancellationToken);

            var result = _mapper.Map<AuthorResult>(author);

            return Ok(result);
        }
    }
}
