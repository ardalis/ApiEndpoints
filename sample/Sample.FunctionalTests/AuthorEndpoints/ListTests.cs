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
    public class ListTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ListTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsTwoAuthors()
        {
            var response = await _client.GetAsync($"/authors");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Author>>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(SeedData.Authors().Count(), result.Count());
        }
    }
}
