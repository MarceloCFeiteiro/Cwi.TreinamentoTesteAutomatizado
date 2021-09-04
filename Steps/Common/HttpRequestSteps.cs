using Cwi.TreinamentoTesteAutomatizado.Controlles;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Cwi.TreinamentoTesteAutomatizado.Models;
using TechTalk.SpecFlow.Assist;
using System.Linq;
using Dapper;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cwi.TreinamentoTesteAutomatizado.Steps.Common
{
    [Binding]
    public class HttpRequestSteps
    {
        private readonly ScenarioContext _scenarioContext;

        private readonly HttpRequestController _httpRequestController;

        public HttpRequestSteps(ScenarioContext scenarioContext, HttpRequestController httpRequestController)
        {
            _scenarioContext = scenarioContext;
            _httpRequestController = httpRequestController;
        }

        [Given(@"seja feita uma chamda do tipo '(.*)' para o endpoint '(.*)' como o corpo da requisição")]
        public async Task DadoSejaFeitaUmaChamdaDoTipoParaOEndpointComoOCorpoDaRequisicao(string httpMethodName, string endpoint, string body)
        {
            _httpRequestController.AddJsonBody(body);

            await _httpRequestController.SendAsync(endpoint, httpMethodName);
        }

        [Given(@"seja feita uma chamada do tipo '(.*)' para o endpoint '(.*)'")]
        public async Task DadoSejaFeitaUmaChamadaDoTipoParaOEndpoint(string httpMethodName, string endpoint)
        {
            await _httpRequestController.SendAsync(endpoint, httpMethodName);
        }


        [Then(@"o código de retorno será '(.*)'")]
        public void EntaoOCodigoDeRetornoSera(int httpStatusCode)
        {
            Assert.AreEqual(httpStatusCode, (int)_httpRequestController.GetResponseHttpStatusCode());
        }

        [Then(@"o conteúdo retornado será")]
        public async Task EntaoOConteudoRetornadoSera(Table table)
        {
            Assert.AreEqual(HttpStatusCode.OK, _httpRequestController.GetResponseHttpStatusCode());

            var expectedEmployeesResponse = await _httpRequestController.GetListTypedResponseBody<Employee>();

            var actalTable = table.CreateSet<Employee>().ToList();

            Assert.AreEqual(expectedEmployeesResponse.Count, actalTable.Count, $"A quantidade de {expectedEmployeesResponse.Count} da request é diferente da quantidade de {actalTable.Count} da tabela.");

            for (int i = 0; i < expectedEmployeesResponse.Count; i++)
            {
                Assert.AreEqual(expectedEmployeesResponse[i], actalTable[i]);
            }
        }

        [Then(@"o conteúdo retornado será generico")]
        public async Task EntaoOConteudoRetornadoSeraGenerico(Table table)
        {
            Assert.AreEqual(HttpStatusCode.OK, _httpRequestController.GetResponseHttpStatusCode());

            var expectedEmployeesResponse = await _httpRequestController.GetResponseBodyContent();

            var actalTable = JsonConvert.SerializeObject(table.CreateDynamicSet()).Replace("'", "");

            Assert.IsTrue(JToken.DeepEquals(JToken.Parse(expectedEmployeesResponse), JToken.Parse(actalTable)),
               $"Conteúdo atual do retorno \n{expectedEmployeesResponse} diferente do esperado \n {actalTable}");
        }

    }
}
