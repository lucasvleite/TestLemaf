using Lemaf.Entities;
using Lemaf.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Lemaf.UnitTest
{
    public class ReservaSalaTest
    {
        private static IReservaSalaService _service;
        private static readonly string[] EntradaDadosSucesso =
        {
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Não"
        };

        [Fact]
        public async Task Quando_Todas_Reservas_Devem_Retornar_SucessoAsync()
        {
            string resposta = await _service.ReservarSalas(EntradaDadosSucesso);
            var resultado = new HistoricoReserva()
            {
                InformacoesReservas = { "ok", "ok", "ok" },
                Reservas =
                {
                    AdicionarReserva(DateTime.Parse("26-05-2019 10:00"), DateTime.Parse("26-05-2019 12:00"), 10, AdicionarSala(1, 10, true, true)),
                    AdicionarReserva(DateTime.Parse("26-05-2019 10:00"), DateTime.Parse("26-05-2019 12:00"), 10, AdicionarSala(2, 10, true, true)),
                    AdicionarReserva(DateTime.Parse("26-05-2019 10:00"), DateTime.Parse("26-05-2019 12:00"), 10, AdicionarSala(6, 10, true, false))
                }
            };

            resposta.Equals(JsonConvert.SerializeObject(resultado));
        }

        private Reserva AdicionarReserva(DateTime dataInicio, DateTime dataFinal, int capacidade, Sala sala) => new Reserva
        {
            DataInicio = dataInicio,
            DataFim = dataFinal,
            QuantidadePessoas = capacidade,
            Sala = sala
        };

        private Sala AdicionarSala(int codigoSala, int capacidade, bool internet, bool tvWebcam) => new Sala
        {
            CodigoSala = codigoSala,
            Capacidade = capacidade,
            PossuiInternet = internet,
            PossuiTvWebcam = tvWebcam
        };
    }
}
