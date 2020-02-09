using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Authors
{
    public class Get : BaseEndpoint<int, AuthorResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public Get(IRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("/authors/{id}")]
        public override ActionResult<AuthorResult> Handle(int id)
        {
            var author = _repository.GetById<Author>(id);

            var result = _mapper.Map<AuthorResult>(author);

            return Ok(result);
        }
    }
}