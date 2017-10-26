using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace MVCMovie.IntegrationTests
{
    public class MovieControllerShould : IClassFixture<TestServerFixture<MvcMovie.Startup>>
    {
        private readonly TestServerFixture<MvcMovie.Startup> _fixture;

        public MovieControllerShould(TestServerFixture<MvcMovie.Startup> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task MovieIndexGetTest()
        {
            var response = await _fixture.Client.GetAsync("/Movies/Index");

            // Fail the test if non-success result
            response.EnsureSuccessStatusCode();

            // Get the response as a string
            string responseHtml = await response.Content.ReadAsStringAsync();

            // Assert on correct content
            Assert.Contains("Index - Movie App", responseHtml);
        }
        [Fact]
        public async Task NotAcceptedCreateWithoutTitle()
        {
            // Get initial response that contains anti forgery tokens
            HttpResponseMessage initialResponse = await _fixture.Client.GetAsync("/Movies/Create");
            var antiForgeryValues = await _fixture.ExtractAntiForgeryValues(initialResponse);
            
            // Create POST request, adding anti forgery cookie and form field
            HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Post, "/Movies/Create");
            postRequest.Headers.Add("Cookie",
                            new CookieHeaderValue(TestServerFixture<MvcMovie.Startup>.AntiForgeryCookieName,
                                                  antiForgeryValues.cookieValue).ToString());

            var formData = new Dictionary<string, string>
            {
                {TestServerFixture<MvcMovie.Startup>.AntiForgeryFieldName, antiForgeryValues.fieldValue},
                //{"ID","1" },
                //{"Title","" },
                {"ReleaseDate","10/12/2016" },
                {"Genre","Romantic Comedy" },
                {"Price","7.99" },
                {"Rating","R" }
            };
            postRequest.Content = new FormUrlEncodedContent(formData);
            HttpResponseMessage postResponse= await _fixture.Client.SendAsync(postRequest);

            // Fail the test if non-success result
            postResponse.EnsureSuccessStatusCode();

            // Get the response as a string
            string responseHtml = await postResponse.Content.ReadAsStringAsync();

            // Assert on correct content
            Assert.Contains("Please provide a title", responseHtml);
        }

    }
}
