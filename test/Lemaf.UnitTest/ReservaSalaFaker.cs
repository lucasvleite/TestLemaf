using Bogus;
using Lemaf.Entities;
using System;

namespace Lemaf.UnitTest
{
    public class ReservaSalaFaker
    {
        private int NumeroAleatorio => new Faker().Random.Int();

        private static string DataAleatoria
        {
            get { return new Faker().Date.Between(DateTime.Now.Date, DateTime.Now.AddDays(100)).ToString().Replace("/", "-").Replace(" ", ";"); }
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
