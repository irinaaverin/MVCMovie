using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MvcMovie;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MVCMovie.IntegrationTest
{
    //[Collection("SystemCollection")]
    public class PingTests
    {
        //public readonly TestContext Context;

        //public PingTests(TestContext context)
        //{
        //    Context = context;
        //}

        //[Fact]
        //public async Task PingReturnsOkResponse()
        //{
        //    var response = await Context.Client.GetAsync("/ping");
        //    response.EnsureSuccessStatusCode();

        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //}
        [Fact]
        public async Task HomeIndexTest()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(
                        @"D:\temp\MvcMovie1\MvcMovie1")
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .UseApplicationInsights();
            TestServer testServer = new TestServer(builder);

            HttpClient client = testServer.CreateClient();
            
            HttpResponseMessage response = await client.GetAsync("/");
            
            // Fail the test if non-success result
            response.EnsureSuccessStatusCode();

            // Get the response as a string
            string responseHtml = await response.Content.ReadAsStringAsync();

            // Assert on correct content
            //Assert.Contains("Home Page", responseHtml);

        }
        //[Fact]
        //public async Task RenderApplicationForm()
        //{

        //    var response = await Context.Client.GetAsync("/");

        //    response.EnsureSuccessStatusCode();

        //    var responseString = await response.Content.ReadAsStringAsync();
        //    //response.StatusCode.Should().Be(HttpStatusCode.OK);
        //}        
    }
}
