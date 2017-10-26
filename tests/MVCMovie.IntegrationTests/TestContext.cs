using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using MvcMovie;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace MVCMovie.IntegrationTest
{
    public class TestContext : IDisposable
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }

        public TestContext()
        {
            SetUpClient();
        }

        private void SetUpClient()
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            var setDir = Path.GetFullPath(Path.Combine(path, @"" ));

            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot(@"D:\temp\MvcMovie1\MvcMovie1")
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .UseApplicationInsights()
                //.PreferHostingUrls(false)
                //.UseUrls("http://*:5000;http://localhost:5001;https://hostname:5002")
                );

            Client = _server.CreateClient();
        }

        public void Dispose()
        {
            _server?.Dispose();
            Client?.Dispose();
        }
    }
}
