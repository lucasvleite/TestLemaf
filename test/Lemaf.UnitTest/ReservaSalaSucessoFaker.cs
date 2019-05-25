using Bogus;
using Lemaf.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Lemaf.UnitTest
{
    public class ReservaSalaSucessoFaker
    {
        private int NumeroAleatorio => new Faker().Random.Int();

        private static string DataAleatoria
        {
            get { return new Faker().Date.Between(DateTime.Now.Date, DateTime.Now.AddDays(100)).ToString().Replace("/", "-").Replace(" ", ";"); }
        }

        public static string DataAleatoriaCorreta
        {
            get
            {
                DateTime dadtaInicio = new Faker().Date.Between(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(40));
                while (dadtaInicio.DayOfWeek.Equals(DayOfWeek.Sunday | DayOfWeek.Saturday))
                    dadtaInicio = new Faker().Date.Between(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(40));
                DateTime dataFinal = dadtaInicio.AddHours(new Faker().Random.Double(1, 8));

                return string.Concat(
                    dadtaInicio.ToString().Replace("/", "-").Replace(" ", ";").Remove(16),
                    ";",
                    dataFinal.ToString().Replace("/", "-").Replace(" ", ";").Remove(16)
                );
            }
        }

        public static string[] EntradaDadosSucesso
        {
            get
            {
                var entradaDados = new List<string>();
                for (int i = 0; i < 100; i++)
                    entradaDados.Add(string.Concat(
                        DataAleatoriaCorreta,
                        new Faker().Random.Int(1,20).ToString(),
                        new Faker().Random.Bool() ? "Sim" : "Não",
                        new Faker().Random.Bool() ? "Sim" : "Não"
                    ));

                return entradaDados.ToArray();
            }
        }

        public static string GetFakerSucesso()
        {
            var historicoReserva = new HistoricoReserva()
            {
                InformacoesReservas = new List<string>(),
                Reservas = new List<Reserva>()
            };

            foreach (var item in EntradaDadosSucesso)
            {
                var entrada = item.Split(";");

                historicoReserva.InformacoesReservas.Add("ok");
                historicoReserva.Reservas.Add(AdicionarReserva(
                    dataInicio: DateTime.Parse(string.Concat(entrada[0]," ",entrada[1])),
                    dataFinal: DateTime.Parse(string.Concat(entrada[2], " ", entrada[3])),
                    capacidade: Convert.ToInt32(entrada[4]),
                    sala: AdicionarSala(1, 10, true, true)
                ));
            };

            return JsonConvert.SerializeObject(historicoReserva);
        }

        public static readonly string[] EntradaDadosExemploDaAvaliacao =
        {
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Não"
        };

        public static string GetFakerExemploDaAvaliacao()
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
