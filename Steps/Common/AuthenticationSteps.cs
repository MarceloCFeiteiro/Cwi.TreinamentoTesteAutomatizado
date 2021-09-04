using Cwi.TreinamentoTesteAutomatizado.Controlles;
using Cwi.TreinamentoTesteAutomatizado.Models;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Cwi.TreinamentoTesteAutomatizado.Steps.Common
{
    [Binding]
    public class AuthenticationSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpRequestController _httpRequestController;
        private readonly IConfiguration _configuration;

        public AuthenticationSteps(ScenarioContext scenarioContext,
            HttpRequestController httpRequestController,
            IConfiguration configuration)
        {
            _scenarioContext = scenarioContext;
            _httpRequestController = httpRequestController;
            this._configuration = configuration;
        }

        [Given(@"que o usuário não esteja autenticado")]
        public void DadoQueOUsuarioNaoEstejaAutenticado()
        {
            _httpRequestController.RemoveHeader("Authorization");
        }

        [Given(@"que o usuário esteja autenticado")]
        public async Task DadoQueOUsuarioEstejaAutenticado()
        {

            _httpRequestController.AddJsonBody(new
            {
                Username = _configuration["Authentication:Username"],
                Password = _configuration["Authentication:Password"]
            });

            await _httpRequestController.SendAsync("v1/public/auth", "Post");

            Assert.AreEqual(HttpStatusCode.OK, _httpRequestController.GetResponseHttpStatusCode());

            var authenticationResponse = await _httpRequestController.GetTypedResponseBody<AuthenticationResponse>();

            _httpRequestController.AddHeader("Authorization", $"Bearer {authenticationResponse.AccessToken}");

        }

    }
}
