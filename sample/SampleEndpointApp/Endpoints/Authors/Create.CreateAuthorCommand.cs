using System.ComponentModel.DataAnnotations;

namespace SampleEndpointApp.Endpoints.Authors
{
    public class CreateAuthorCommand
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string PluralsightUrl { get; set; } = null!;
        public string? TwitterAlias { get; set; }
    }
}
