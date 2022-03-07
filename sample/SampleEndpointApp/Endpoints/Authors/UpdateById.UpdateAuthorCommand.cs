using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SampleEndpointApp.Endpoints.Authors;

public class UpdateAuthorCommandById
{
  [Required]
  [FromRoute]
  public int Id { get; set; }

  [FromBody]
  public UpdateDetails Details { get; set; } = null!;

  public class UpdateDetails
  {
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string TwitterAlias { get; set; } = null!;
  }

}
