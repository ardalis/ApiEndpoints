using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Authors
{
    public class Create : BaseEndpoint<CreateAuthorCommand, CreateAuthorResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public Create(IRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost("/authors")]
        public override ActionResult<CreateAuthorResult> Handle([FromBody]CreateAuthorCommand request)
        {
            var author = new Author();
            _mapper.Map(request, author);
            _repository.Add(author);

            var result = _mapper.Map<CreateAuthorResult>(author);
            return Ok(result);
        }
    }
}