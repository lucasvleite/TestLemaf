using Lemaf.Services.Interfaces;
using Lemaf.Services.Services;
using Newtonsoft.Json;
using Softplan.ACL.WebApi.UnitTest;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Lemaf.UnitTest
{
    public class ReservaSalaTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        private static IReservaService _service;

        public ReservaSalaTest(TestServerFixture fixture)
        {
            _fixture = fixture;
            _service = new ReservaService();
        }

        [Fact]
        public async Task Quando_Todas_Reservas_Do_Exemplo_Devem_Retornar_Sucesso()
        {
            string resposta = await _service.ReservarSalas(ReservaSalaSucessoFaker.EntradaDadosExemploDaAvaliacao);
            var resultado = ReservaSalaSucessoFaker.GetFakerExemploDaAvaliacao();

            resposta.Equals(JsonConvert.SerializeObject(resultado));
        }

        [Fact]
        public async Task Quando_Todas_Reservas_Devem_Retornar_Sucesso()
        {
            string resposta = await _service.ReservarSalas(ReservaSalaSucessoFaker.EntradaDadosSucesso);
            var resultado = ReservaSalaSucessoFaker.GetFakerSucesso(ReservaSalaSucessoFaker.EntradaDadosSucesso);

            resposta.Equals(JsonConvert.SerializeObject(resultado));
        }
    }
}
