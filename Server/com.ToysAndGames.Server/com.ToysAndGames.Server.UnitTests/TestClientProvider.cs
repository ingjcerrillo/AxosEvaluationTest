using com.ToysAndGames.Server.WebAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace com.ToysAndGames.Server.UnitTests
{
    /// <summary>
    /// Generates a new disposable HttpClient for testing.
    /// </summary>
    class TestClientProvider : IDisposable
    {
        private TestServer server;
        public HttpClient Client { get; private set; }

        public TestClientProvider()
        {
            this.server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            this.Client = server.CreateClient();
        }

        public void Dispose()
        {
            server?.Dispose();
            Client?.Dispose();
        }
    }
}
