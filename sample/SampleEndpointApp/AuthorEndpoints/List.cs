using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Collections.Generic;
using System.Linq;

namespace SampleEndpointApp.Authors
{
    public class List : BaseEndpoint<List<AuthorListResult>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public List(IRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("/authors")]
        public override ActionResult<List<AuthorListResult>> Handle()
        {
            var result = _repository.List<Author>()
                .Select(i => _mapper.Map<AuthorListResult>(i));

            return Ok(result);
        }
    }
}