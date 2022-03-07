using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Endpoints.Authors;

// Show how to use route and body parameters
// See: https://github.com/ardalis/ApiEndpoints/issues/161
public class UpdateById : EndpointBaseAsync
    .WithRequest<UpdateAuthorCommandById>
    .WithActionResult<UpdatedAuthorByIdResult>
{
  private readonly IAsyncRepository<Author> _repository;
  private readonly IMapper _mapper;

  public UpdateById(IAsyncRepository<Author> repository,
      IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  /// <summary>
  /// Updates an existing Author
  /// </summary>
  [HttpPut("api/[namespace]/{id}")]
  public override async Task<ActionResult<UpdatedAuthorByIdResult>> HandleAsync([FromMultiSource]UpdateAuthorCommandById request,
    CancellationToken cancellationToken)
  {
    var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

    if (author is null) return NotFound();

    author.Name = request.Details.Name;
    author.TwitterAlias = request.Details.TwitterAlias;

    await _repository.UpdateAsync(author, cancellationToken);

    var result = _mapper.Map<UpdatedAuthorByIdResult>(author);
    return result;
  }
}
