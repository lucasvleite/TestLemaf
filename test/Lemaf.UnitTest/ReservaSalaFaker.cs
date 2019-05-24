using Lemaf.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lemaf.UnitTest
{
    public class ReservaSalaFaker
    {
        public static readonly string[] EntradaDadosSucesso =
        {
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Não"
        };

        public static string GetResultadoSucesso()
        {
            var historicoReserva = new HistoricoReserva()
            {
                InformacoesReservas = new List<string> { "ok", "ok", "ok" },
                Reservas = new List<Reserva>
                {
                    AdicionarReserva(DateTime.Parse("26-05-2019 10:00"), DateTime.Parse("26-05-2019 12:00"), 10, AdicionarSala(1, 10, true, true)),
                    AdicionarReserva(DateTime.Parse("26-05-2019 10:00"), DateTime.Parse("26-05-2019 12:00"), 10, AdicionarSala(2, 10, true, true)),
                    AdicionarReserva(DateTime.Parse("26-05-2019 10:00"), DateTime.Parse("26-05-2019 12:00"), 10, AdicionarSala(6, 10, true, false))
                }
            };

            return JsonConvert.SerializeObject(historicoReserva);
        }

        private static Reserva AdicionarReserva(DateTime dataInicio, DateTime dataFinal, int capacidade, Sala sala) => new Reserva
        {
            DataInicio = dataInicio,
            DataFim = dataFinal,
            QuantidadePessoas = capacidade,
            Sala = sala
        };

        private static Sala AdicionarSala(int codigoSala, int capacidade, bool internet, bool tvWebcam) => new Sala
        {
            CodigoSala = codigoSala,
            Capacidade = capacidade,
            PossuiComputador = true,
            PossuiInternet = internet,
            PossuiTvWebcam = tvWebcam
        };
    }
}
