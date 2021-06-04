using Newtonsoft.Json;
using SampleEndpointApp;
using SampleEndpointApp.Authors;
using SampleEndpointApp.DataAccess;
using SampleEndpointApp.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints
{
    public class DeleteEndpoint : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public DeleteEndpoint(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task DeleteAnExistingAuthor()
        {
            int existingAuthorId = 2;
            string route = DeleteAuthorRequest.ROUTE.Replace("{id}", existingAuthorId.ToString());
            var response = await _client.DeleteAsync(route);
            response.EnsureSuccessStatusCode();
            
            var listResponse = await _client.GetAsync($"/authors");
            listResponse.EnsureSuccessStatusCode();
            var stringListResponse = await listResponse.Content.ReadAsStringAsync();
            var listResult = JsonConvert.DeserializeObject<IEnumerable<Author>>(stringListResponse);

            Assert.True(listResult.Count() <= 2);
        }

        [Fact]
        public Task GivenLongRunningDeleteRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
        {
            // Arrange, generate a token source that times out instantly
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(0));

            // Act
            int existingAuthorId = 2;
            string route = DeleteAuthorRequest.ROUTE.Replace("{id}", existingAuthorId.ToString());
            var request = _client.DeleteAsync(route, tokenSource.Token);

            // Assert
            return Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
        }
    }
}
