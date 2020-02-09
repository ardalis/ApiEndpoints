using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Authors
{
    public class Delete : BaseEndpoint<int, DeletedAuthorResult>
    {
        private readonly IRepository _repository;

        public Delete(IRepository repository)
        {
            _repository = repository;
        }

        [HttpDelete("/authors/{id}")]
        public override ActionResult<DeletedAuthorResult> Handle(int id)
        {
            var author = _repository.GetById<Author>(id);
            _repository.Delete<Author>(author);

            return Ok(new DeletedAuthorResult { DeletedAuthorId = id });
        }
    }
}