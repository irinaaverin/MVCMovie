using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MVCMovie.IntegrationTests
{
    public class HomeControllerShould : IClassFixture<TestServerFixture<MvcMovie.Startup>>
    {
        private readonly HttpClient _client;
        public HomeControllerShould(TestServerFixture<MvcMovie.Startup> fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task IndexGetLoadsHomePage()
        {

            var response = await _client.GetAsync("/");

            // Fail the test if non-success result
            response.EnsureSuccessStatusCode();

            // Get the response as a string
            string responseHtml = await response.Content.ReadAsStringAsync();

            // Assert on correct content
            Assert.Contains("Home Page", responseHtml);

        }
        [Fact]
        public async Task AboutGetLoadsAboutPage()
        {            
            var response = await _client.GetAsync("/Home/About");

            // Fail the test if non-success result
            response.EnsureSuccessStatusCode();

            // Get the response as a string
            string responseHtml = await response.Content.ReadAsStringAsync();

            // Assert on correct content
            Assert.Contains("Your application description page.", responseHtml);
            Assert.Contains("About", responseHtml);

        }
    }
}
