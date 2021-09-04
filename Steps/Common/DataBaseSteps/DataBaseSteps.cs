using Cwi.TreinamentoTesteAutomatizado.Controlles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Cwi.TreinamentoTesteAutomatizado.Steps.Common.DataBaseSteps
{
    [Binding]
    public sealed class DataBaseSteps
    {

        private readonly ScenarioContext _scenarioContext;
        private readonly PostgreDataBaseController _postgreDataBaseController;

        public DataBaseSteps(ScenarioContext scenarioContext, PostgreDataBaseController postgreDataBaseController)
        {
            _scenarioContext = scenarioContext;
            this._postgreDataBaseController = postgreDataBaseController;
        }

        [Given(@"que a base de dados esteja limpa")]
        public async Task DadoQueABaseDeDadosEstejaLimpa()
        {
            await _postgreDataBaseController.ClearDataBase();
        }

        [Then(@"o registro estará disponivel na tabela '(.*)' da base de dados")]
        public async Task EntaoORegistroEstaraDisponivelNaTabelaDaBaseDeDados(string tableName, Table table)
        {
            var currenItens = await _postgreDataBaseController.SelectFrom(tableName, table);

            Assert.NotZero(currenItens.Count(), $"Não foram encontrados registros na tabela {tableName}.");

            var actualJsonResponse = JsonConvert.SerializeObject(currenItens);

            var expectedJsonConvertResponse = JsonConvert.SerializeObject(table.CreateDynamicSet()).Replace("'", "");

            Assert.IsTrue(JToken.DeepEquals(JToken.Parse(actualJsonResponse), JToken.Parse(expectedJsonConvertResponse)),
                $"Conteúdo atual do retorno \n{actualJsonResponse} diferente do esperado \n {expectedJsonConvertResponse}");
        }

        [Given(@"os registros sejam inseridos na tabela '(.*)' da base de dados")]
        public async Task DadoOsRegistrosSejamInseridosNaTabelaDaBaseDeDados(string tableName, Table table)
        {
           await _postgreDataBaseController.InsertFrom(tableName, table);
        }

    }
}
