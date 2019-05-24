using Lemaf.Services.Interfaces;
using Lemaf.Services.Services;
using Newtonsoft.Json;
using Softplan.ACL.WebApi.UnitTest;
using System.Threading.Tasks;
using Xunit;

namespace Lemaf.UnitTest
{
    public class ReservaSalaTest : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        private static IReservaSalaService _service;

        public ReservaSalaTest(TestServerFixture fixture)
        {
            _fixture = fixture;
            _service = new ReservaSalaService(fixture.Resolve<IReservaSalaService>());
        }

        [Fact]
        public async Task Quando_Todas_Reservas_Devem_Retornar_SucessoAsync()
        {
            string resposta = await _service.ReservarSalas(ReservaSalaFaker.EntradaDadosSucesso);
            var resultado = ReservaSalaFaker.GetResultadoSucesso();

            resposta.Equals(JsonConvert.SerializeObject(resultado));
        }
    }
}
