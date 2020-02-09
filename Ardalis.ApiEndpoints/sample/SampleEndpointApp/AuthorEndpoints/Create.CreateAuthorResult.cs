using System.ComponentModel.DataAnnotations;

namespace SampleEndpointApp.Authors
{
    public class CreateAuthorResult : CreateAuthorCommand
    {
        public int Id { get; set; }
    }
}