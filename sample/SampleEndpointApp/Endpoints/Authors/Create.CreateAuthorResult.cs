using System.ComponentModel.DataAnnotations;

namespace SampleEndpointApp.Endpoints.Authors
{
    public class CreateAuthorResult : CreateAuthorCommand
    {
        public int Id { get; set; }
    }
}
