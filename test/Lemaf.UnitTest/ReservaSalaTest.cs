using Lemaf.Entities;
using Lemaf.Services.Interfaces;
using Lemaf.Services.Services;
using Newtonsoft.Json;
using Softplan.ACL.WebApi.UnitTest;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Lemaf.UnitTest
{
    public class ReservaSalaTest : IClassFixture<TestServerFixture>
    {
        private const string Ok = "ok";
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
            string resposta = await _service.ReservarSalasAsync(ReservaSalaSucessoFaker.EntradaDadosExemploDaAvaliacao);
            var resultado = ReservaSalaSucessoFaker.GetFakerExemploDaAvaliacao();

            resposta.Equals(JsonConvert.SerializeObject(resultado));
        }

        [Fact]
        public async Task Quando_Todas_Reservas_Devem_Retornar_Sucesso()
        {
            string resposta = await _service.ReservarSalasAsync(ReservaSalaSucessoFaker.EntradaDadosSucesso);
            var historico = JsonConvert.DeserializeObject<HistoricoReserva>(resposta);

            Assert.NotEmpty(resposta);
            int i = 0;
            foreach (Reserva item in historico.Reservas)
            {
                historico.InformacoesReservas.ElementAt(i).Equals(Ok);
                Assert.NotNull(historico.Reservas.ElementAt(i));

                i++;
            }
        }

        [Fact]
        public async Task Quando_Reservas_Data_Inicial_Incorreta_Devem_Retornar_Erro()
        {
            string resposta = await _service.ReservarSalasAsync(ReservaSalaFaker.EntradaDadosErroDataInicial);

            var historico = JsonConvert.DeserializeObject<HistoricoReserva>(resposta);

            Assert.NotEmpty(resposta);
            int i = 0;
            foreach (Reserva item in historico.Reservas)
            {
                if (item == (null))
                    Assert.NotEqual(Ok, historico.InformacoesReservas.ElementAt(i));
                else
                    historico.InformacoesReservas.ElementAt(i).Equals(Ok);

                i++;
            }
        }

        [Fact]
        public async Task Quando_Reservas_Data_Final_Incorreta_Devem_Retornar_Erro()
        {
            string resposta = await _service.ReservarSalasAsync(ReservaSalaFaker.EntradaDadosErroDataFinal);

            var historico = JsonConvert.DeserializeObject<HistoricoReserva>(value: resposta);

            Assert.NotEmpty(resposta);
            int i = 0;
            foreach (Reserva item in historico.Reservas)
            {
                if (item == (null))
                    Assert.NotEqual(Ok, historico.InformacoesReservas.ElementAt(i));
                else
                    historico.InformacoesReservas.ElementAt(i).Equals(Ok);

                i++;
            }
        }

        [Fact]
        public async Task Quando_Reservas_Datas_Incorretas_Devem_Retornar_Erro()
        {
            string resposta = await _service.ReservarSalasAsync(ReservaSalaFaker.EntradaDadosMista);

            var historico = JsonConvert.DeserializeObject<HistoricoReserva>(value: resposta);

            Assert.NotEmpty(resposta);
            int i = 0;
            foreach (Reserva item in historico.Reservas)
            {
                if (item == (null))
                    Assert.NotEqual(Ok, historico.InformacoesReservas.ElementAt(i));
                else
                    historico.InformacoesReservas.ElementAt(i).Equals(Ok);

                i++;
            }
        }
    }
}
