using TechTalk.SpecFlow;
using NUnit.Framework;
using ConsolidadorTeste.Projeto_Pages;

namespace ConsolidadorTeste.Projeto_Steps
{
    [Binding]
    class Google_step : Google_page
    {

        [Given(@"acesso ao site '(.*)'")]
        public void GivenAcessoAoSite(string value)
        {
            Assert.IsTrue(startBrowser(value));
        }

        [When(@"valido botao pesquisa com o nome de '(.*)'")]
        public void WhenValidoBotaoPesquisaComONomeDe(string value)
        {
            Assert.IsTrue(inserirNome(string.Empty));
        }

        [Then(@"valido tbm botao sorte com o nome de '(.*)'")]
        public void ThenValidoTbmBotaoSorteComONomeDe(string value)
        {
            Assert.IsTrue(inserirNome(string.Empty));
        }
    }
}
