using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sample.FunctionalTests.Models;
using SampleEndpointApp;
using SampleEndpointApp.DomainModel;
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
            string route = Routes.Authors.Delete(existingAuthorId);
            var response = await _client.DeleteAsync(route);
            response.EnsureSuccessStatusCode();

            var listResponse = await _client.GetAsync(Routes.Authors.List());
            listResponse.EnsureSuccessStatusCode();
            var stringListResponse = await listResponse.Content.ReadAsStringAsync();
            var listResult = JsonConvert.DeserializeObject<IEnumerable<Author>>(stringListResponse);

            Assert.True(listResult.Count() <= 2);
        }

        [Fact]
        public Task GivenLongRunningDeleteRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
        {
            // Arrange, generate a token source that times out instantly
            var tokenSource = new CancellationTokenSource(TimeSpan.Zero);

            // Act
            int existingAuthorId = 2;
            string route = Routes.Authors.Delete(existingAuthorId);
            var request = _client.DeleteAsync(route, tokenSource.Token);

            // Assert
            return Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
        }
    }
}
