using Lemaf.Entities;
using Lemaf.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lemaf.Services.Services
{
    public class ReservaSalaService : IReservaSalaService
    {
        private List<Sala> Salas { get; set; }

        public async Task<string> ReservarSalas(string[] entradaDados)
        {
            CarregarSalas();
            var HistoricoReserva = new HistoricoReserva();

            return JsonConvert.SerializeObject(HistoricoReserva);
        }

        private void CarregarSalas()
        {
            Salas = new List<Sala>();
            for (int i = 1; i <= 5; i++)
            {
                NovaSala(i, 10, true, true, true);
            }

            NovaSala(6, 10, false, true, false);
            NovaSala(7, 10, false, true, false);

            NovaSala(8, 3, true, true, true);
            NovaSala(9, 3, true, true, true);
            NovaSala(10, 3, true, true, true);

            NovaSala(11, 20, false, false, false);
            NovaSala(12, 20, false, false, false);
        }

        private void NovaSala(int codigoSala, int capacidade, bool possuiComputador, bool possuiInternet, bool possuiTvWebcam) =>
            Salas.Add(new Sala
            {
                CodigoSala = codigoSala,
                Capacidade = capacidade,
                PossuiComputador = possuiComputador,
                PossuiInternet = possuiInternet,
                PossuiTvWebcam = possuiTvWebcam
            });

    }
}
