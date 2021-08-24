using System.ComponentModel.DataAnnotations;

namespace SampleEndpointApp.Authors
{
    public class CreateAuthorCommand
    {
        public const string ROUTE = "/authors";

        [Required]
        public string Name { get; set; }
        [Required]
        public string PluralsightUrl { get; set; }
        public string TwitterAlias { get; set; }
    }
}
