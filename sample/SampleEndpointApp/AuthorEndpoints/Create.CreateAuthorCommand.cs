using System.ComponentModel.DataAnnotations;

namespace SampleEndpointApp.Authors
{
    public class CreateAuthorCommand
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PluralsightUrl { get; set; }
        public string TwitterAlias { get; set; }
    }
}