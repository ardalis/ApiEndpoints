using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Threading.Tasks;

namespace SampleEndpointApp.Authors
{
    public class Create : BaseAsyncEndpoint<CreateAuthorCommand, CreateAuthorResult>
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public Create(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("/authors")]
        public override async Task<ActionResult<CreateAuthorResult>> HandleAsync([FromBody]CreateAuthorCommand request)
        {
            var author = new Author();
            _mapper.Map(request, author);
            await _repository.AddAsync(author);

            var result = _mapper.Map<CreateAuthorResult>(author);
            return Ok(result);
        }
    }
}