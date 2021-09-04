using BoDi;
using Cwi.TreinamentoTesteAutomatizado.Controlles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.IO;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace Cwi.TreinamentoTesteAutomatizado
{
    [Binding]
    public class Startup
    {
        private readonly IObjectContainer _objectContainer;

        public Startup(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void DependencyInjection()
        {
            var configuration = GetConfiguration();
            var httpRequestController = new HttpRequestController(GetHttpClientFactory(), configuration["ExampleApiURL"]);
            var postgreDataBaseController = new PostgreDataBaseController(new NpgsqlConnection(configuration["ConnectionStrings:ExampleDb"]));


            _objectContainer.RegisterInstanceAs<IConfiguration>(configuration);
            _objectContainer.RegisterInstanceAs<HttpRequestController>(httpRequestController);
            _objectContainer.RegisterInstanceAs<PostgreDataBaseController>(postgreDataBaseController);
        }

        private IConfiguration GetConfiguration()
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings{envName}.json", optional: true).Build();
        }

        private IHttpClientFactory GetHttpClientFactory()
        {
            return new ServiceCollection()
                .AddHttpClient()
                .BuildServiceProvider()
                .GetRequiredService<IHttpClientFactory>();
        }

    }
}
