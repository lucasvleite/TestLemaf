using System;
using System.Net.Http;
using Lemaf.App;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Softplan.ACL.WebApi.UnitTest
{
    public class TestServerFixture
    {
        private readonly TestServer _testServer;
        public TestServerFixture()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Test")
            .UseStartup<Startup>()
            //.UseSetting("DATABASE_PROVIDER", "INMEMORY")
            ;

            _testServer = new TestServer(builder);
            Client = _testServer.CreateClient();
            Services = _testServer.Host.Services;
        }

        private HttpClient Client { get; }

        public IServiceProvider Services { get; }

        public T Resolve<T>() => Services.GetService<T>();

        public void Dispose()
        {
            Client.Dispose();
            _testServer.Dispose();
        }
    }
}
