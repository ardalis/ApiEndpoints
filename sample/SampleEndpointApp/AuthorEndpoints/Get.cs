using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Threading.Tasks;

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
        public override async Task<ActionResult<AuthorResult>> HandleAsync(int id)
        {
            var author = await _repository.GetByIdAsync(id);

            var result = _mapper.Map<AuthorResult>(author);

            return Ok(result);
        }
    }
}