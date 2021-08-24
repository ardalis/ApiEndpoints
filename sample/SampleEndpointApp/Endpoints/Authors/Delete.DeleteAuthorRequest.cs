namespace SampleEndpointApp.Authors
{
    public class DeleteAuthorRequest
    {
        public const string ROUTE = "/authors/{id}";

        public int Id { get; set; }
    }
}
