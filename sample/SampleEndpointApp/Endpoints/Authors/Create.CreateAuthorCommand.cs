using System.ComponentModel.DataAnnotations;

namespace SampleEndpointApp.Endpoints.Authors
{
    public class CreateAuthorCommand
    {
        public const string ROUTE = "/authors";

        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string PluralsightUrl { get; set; } = null!;
        public string? TwitterAlias { get; set; }
    }
}
