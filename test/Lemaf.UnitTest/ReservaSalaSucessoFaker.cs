using Bogus;
using Lemaf.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Lemaf.Services.Services;

namespace Lemaf.UnitTest
{
    public class ReservaSalaSucessoFaker
    {
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

        public static string[] EntradaDadosSucesso {
            get {
                HistoricoReserva  historicoReserva= new HistoricoReserva(){
                    InformacoesReservas = new List<string>(),
                    Reservas = new List<Reserva>()
                };

                var entradaDados = new List<string>();

                while(entradaDados.Count <= 50)
                {
                    Reserva reserva = GerarReservaAleatoria(historicoReserva);

                    if (reserva != null)
                    {
                        historicoReserva.Reservas.Add(reserva);
                        historicoReserva.InformacoesReservas.Add("ok");

                        entradaDados.Add(ConverterReservaParaString(reserva.DataInicio, reserva.DataFim, reserva.QuantidadePessoas.Value,
                            reserva.Sala.PossuiInternet, reserva.Sala.PossuiTvWebcam));
                    }
                }

                return entradaDados.ToArray();
            }
        }

        
        public static string GetFakerSucesso(string[] EntradaDadosSucesso)
        {
            HistoricoReserva  historicoReserva= new HistoricoReserva(){
                InformacoesReservas = new List<string>(),
                Reservas = new List<Reserva>()
            };

            foreach (var item in EntradaDadosSucesso)
            {
                var entrada = item.Split(";");

                var reserva = VerificarReserva(
                    historicoReserva,
                    DateTime.Parse(string.Concat(entrada[0], " ", entrada[1])),
                    DateTime.Parse(string.Concat(entrada[2], " ", entrada[3])),
                    Convert.ToInt32(entrada[4]),
                    entrada[5].Equals("Sim") ? true : false,
                    entrada[6].Equals("Sim") ? true : false
                );

                if (reserva != null)
                {
                    historicoReserva.Reservas.Add(reserva);
                    historicoReserva.InformacoesReservas.Add("ok");
                }
            };

            return JsonConvert.SerializeObject(historicoReserva);
        }


        private static Reserva GerarReservaAleatoria(HistoricoReserva historicoReserva)
        {
            DateTime dataInicio = new Faker().Date.Between(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(40));
            while (dataInicio.DayOfWeek.Equals(DayOfWeek.Sunday | DayOfWeek.Saturday))
                dataInicio = new Faker().Date.Between(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(40));

            return VerificarReserva(
                historicoReserva,
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

        private static Sala VerificarSalaDisponivel(HistoricoReserva historicoReserva, Reserva reserva, bool possuiInternet, bool possuiTvWebcam)
        {
            var copiaReserva = reserva;
            var salasAtendem = new SalaService().
                VerificarSalasAtendemNecessidade(copiaReserva.QuantidadePessoas.Value, possuiInternet, possuiTvWebcam);

            foreach (var sala in salasAtendem)
            {
                copiaReserva.Sala = sala;
                if (historicoReserva.Reservas == null)
                    return sala;
                else if (!historicoReserva.Reservas.Contains(copiaReserva))
                    return sala;
            }
            return null;
        }

        private static Reserva VerificarReserva(HistoricoReserva historicoReserva, DateTime dataInicio, DateTime dataFinal,
            int quantidadePessoas, bool possuiInternet, bool possuiTvWebcam)
        {
            Reserva reserva = AdicionarReserva(
                    dataInicio,
                    dataFinal,
                    quantidadePessoas,
                    null
                );

            var sala = VerificarSalaDisponivel(
                historicoReserva,
                reserva,
                possuiInternet,
                possuiTvWebcam
            );
            
            if(sala == null)
            {
                return null;
            }
            else
            {
                reserva.Sala = sala;
                return reserva;
            }
        }
    }
}
