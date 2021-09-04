using Cwi.TreinamentoTesteAutomatizado.Controlles;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Cwi.TreinamentoTesteAutomatizado.Steps
{
    [Binding]
    [Scope(Tag = "CreateEmployee")]
    public sealed class CreateEmployeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpRequestController _httpRequestController;

        public CreateEmployeeSteps(ScenarioContext scenarioContext, HttpRequestController httpRequestController)
        {
            _scenarioContext = scenarioContext;
            this._httpRequestController = httpRequestController;
        }

        [Given(@"que seja solicitado a criação de um novo funcionário")]
        public async Task DadoQueSejaSolicitadoACriacaoDeUmNovoFuncionarioAsync()
        {
            _httpRequestController.AddJsonBody(new { Name = "Funcionário 1", Email = "funcionario1@empresa.com" });

            await _httpRequestController.SendAsync("v1/employees", "Post");
        }

        [Given(@"que seja solicitado a criação de um novo funcionário sem o preenchimento do campos obigatórios")]
        public async Task DadoQueSejaSolicitadoACriacaoDeUmNovoFuncionarioSemOPreenchimentoDoCamposObigatorios()
        {
            _httpRequestController.AddJsonBody(new { });

            await _httpRequestController.SendAsync("v1/employees", "Post");
        }

        [Then(@"o funcionário será cadastrado")]
        public void EntaoOFuncionarioSeraCadastrado()
        {
            Assert.AreEqual(HttpStatusCode.Created, _httpRequestController.GetResponseHttpStatusCode());
        }

        [Then(@"o funcionário não será cadastrado")]
        public void EntaoOFuncionarioNaoSeraCadastrado()
        {
            Assert.AreNotEqual(HttpStatusCode.Created, _httpRequestController.GetResponseHttpStatusCode());
        }

        [Then(@"será retornado uma mensagem de falha de autenticação")]
        public void EntaoSeraRetornadoUmaMensagemDeFalhaDeAutenticacao()
        {
            Assert.AreEqual(HttpStatusCode.Unauthorized, _httpRequestController.GetResponseHttpStatusCode());
        }

        [Then(@"será retornado uma mensagem de falha de preenchimento de campos obrigatórios")]
        public void EntaoSeraRetornadoUmaMensagemDeFalhaDePreenchimentoDeCamposObrigatorios()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, _httpRequestController.GetResponseHttpStatusCode());
        }


    }
}
