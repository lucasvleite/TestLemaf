using Bogus;
using Lemaf.Entities;
using Lemaf.Services.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemaf.UnitTest
{
    public class ReservaSalaSucessoFaker
    {
        private const int tentativasEntradaDados = 100;

        public static readonly string[] EntradaDadosExemploDaAvaliacao =
        {
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Não"
        };

        private static HistoricoReserva HistoricoReserva =
            new HistoricoReserva()
            {
                InformacoesReservas = new List<string>(),
                Reservas = new List<Reserva>()
            };

        public static string GetFakerExemploDaAvaliacao()
        {
            HistoricoReserva = new HistoricoReserva()
            {
                InformacoesReservas = new List<string> { "ok", "ok", "ok" },
                Reservas = new List<Reserva>
                {
                    AdicionarReserva(DateTime.Parse("26-05-2019 10:00"), DateTime.Parse("26-05-2019 12:00"), 10, AdicionarSala(1, 10, true, true)),
                    AdicionarReserva(DateTime.Parse("26-05-2019 10:00"), DateTime.Parse("26-05-2019 12:00"), 10, AdicionarSala(2, 10, true, true)),
                    AdicionarReserva(DateTime.Parse("26-05-2019 10:00"), DateTime.Parse("26-05-2019 12:00"), 10, AdicionarSala(6, 10, true, false))
                }
            };

            return JsonConvert.SerializeObject(HistoricoReserva);
        }

        public static string[] EntradaDadosSucesso {
            get
            {
                var entradaDados = new List<string>();

                for (int i = 0; i < tentativasEntradaDados; i++)
                {
                    Reserva reserva = GerarReservaAleatoria();

                    if (reserva != null)
                    {
                        HistoricoReserva.Reservas.Add(reserva);
                        HistoricoReserva.InformacoesReservas.Add("ok");

                        entradaDados.Add(ConverterReservaParaString(reserva.DataInicio, reserva.DataFinal, reserva.QuantidadePessoas,
                            reserva.Sala.PossuiInternet, reserva.Sala.PossuiTvWebcam));
                    }
                }

                return entradaDados.ToArray();
            }
        }

        private static Reserva GerarReservaAleatoria()
        {
            DateTime dataInicio = new Faker().Date.Between(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(40));
            while (dataInicio.DayOfWeek.Equals(DayOfWeek.Sunday | DayOfWeek.Saturday))
                dataInicio = new Faker().Date.Between(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(40));

            return VerificarReserva(
                dataInicio,
                dataInicio.AddHours(new Faker().Random.Double(1, 8)),
                new Faker().Random.Int(1, 20),
                new Faker().Random.Bool(),
                new Faker().Random.Bool());
        }

        private static string ConverterReservaParaString(DateTime dataInicio, DateTime dataFinal, int quantidadePessoas, bool possuiInternet, bool possuiTvWebcam)
        {
            return string.Concat(
                dataInicio.ToString().Replace("/", "-").Replace(" ", ";").Remove(16), ";",
                dataFinal.ToString().Replace("/", "-").Replace(" ", ";").Remove(16), ";",
                quantidadePessoas.ToString(), ";",
                possuiInternet ? "Sim" : "Não", ";",
                possuiTvWebcam ? "Sim" : "Não");
        }

        private static Reserva AdicionarReserva(DateTime dataInicio, DateTime dataFinal, int capacidade, Sala sala) => new Reserva
        {
            DataInicio = dataInicio,
            DataFinal = dataFinal,
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

        private static Reserva VerificarReserva(DateTime dataInicio, DateTime dataFinal, int quantidadePessoas,
            bool possuiInternet, bool possuiTvWebcam)
        {
            Reserva reserva = AdicionarReserva(dataInicio, dataFinal, quantidadePessoas, null);

            reserva.Sala = VerificarSalaDisponivel(reserva, possuiInternet, possuiTvWebcam);
            
            return (reserva.Sala == null) ? null : reserva;
        }

        private static Sala VerificarSalaDisponivel(Reserva reserva, bool possuiInternet, bool possuiTvWebcam)
        {
            var copiaReserva = reserva;
            var salasAtendem = new SalaService().
                VerificarSalasAtendemNecessidade(copiaReserva.QuantidadePessoas, possuiInternet, possuiTvWebcam);

            foreach (var sala in salasAtendem)
            {
                copiaReserva.Sala = sala;
                 if (HistoricoReserva.Reservas.Equals(null) | !ConflitoHorario(copiaReserva) )
                    return sala;
            }
            return null;
        }

        private static bool ConflitoHorario(Reserva reserva)
        {
            var verificador = HistoricoReserva.Reservas
                .Where(x => x.Sala.CodigoSala == reserva.Sala.CodigoSala & (x.DataInicio > reserva.DataFinal | reserva.DataInicio > x.DataFinal)
            ).ToList();

            return (verificador.Count.Equals(0)) ? false : true;
        }
    }
}
