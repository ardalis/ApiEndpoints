using Newtonsoft.Json;
using SampleEndpointApp;
using SampleEndpointApp.DataAccess;
using SampleEndpointApp.DomainModel;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints
{
    public class ListPaginatedEndpoint : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ListPaginatedEndpoint(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Page1PerPage1_ShouldReturnFirstAuthor()
        {
            var response = await _client.GetAsync($"/authors?perPage=1&page=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Author>>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
        }
    }
}
