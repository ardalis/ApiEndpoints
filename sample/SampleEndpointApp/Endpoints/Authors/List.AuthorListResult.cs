namespace SampleEndpointApp.Endpoints.Authors
{
    public class AuthorListResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? TwitterAlias { get; set; }
    }
}
