using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MvcMovie;
using System;
using System.Net.Http;
using Xunit;

namespace MVCMovie.IntegrationTest
{
    public class MVCMoviesShould
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }

        public MVCMoviesShould()
        {
            SetUpClient();
        }

        private void SetUpClient()
        {
            _server = new TestServer(new WebHostBuilder()
                            .UseStartup<Startup>());

            Client = _server.CreateClient();
        }

        [Fact]
        public void RenderApplicationForm()
        {
            int a = 8;
            //var response = await Client.GetAsync("/Index");
            //response.EnsureSuccessStatusCode();
            //var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
