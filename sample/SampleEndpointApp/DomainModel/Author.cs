namespace SampleEndpointApp.DomainModel
{
    public class Author : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string PluralsightUrl { get; set; } = null!;
        public string? TwitterAlias { get; set; }
    }
}
