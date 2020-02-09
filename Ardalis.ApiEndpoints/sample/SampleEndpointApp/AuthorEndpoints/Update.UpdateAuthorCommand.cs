using System.ComponentModel.DataAnnotations;

namespace SampleEndpointApp.Authors
{
    public class UpdateAuthorCommand
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}