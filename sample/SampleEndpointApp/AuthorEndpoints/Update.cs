using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Authors
{
    public class Update : BaseEndpoint<UpdateAuthorCommand, UpdatedAuthorResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public Update(IRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPut("/authors")]
        public override ActionResult<UpdatedAuthorResult> Handle([FromBody]UpdateAuthorCommand request)
        {
            var author = _repository.GetById<Author>(request.Id);
            _mapper.Map(request, author);
            _repository.Update(author);

            var result = _mapper.Map<UpdatedAuthorResult>(author);
            return Ok(result);
        }
    }
}