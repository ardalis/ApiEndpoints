namespace SampleEndpointApp.Endpoints.Authors;

public class UpdatedAuthorResult
{
  public string Id { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string PluralsightUrl { get; set; } = null!;
  public string? TwitterAlias { get; set; }
}
