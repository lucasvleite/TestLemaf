using Lemaf.Entities;
using Lemaf.Services.Interfaces;
using Lemaf.Services.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lemaf.Services.Services
{
    public class ReservaSalaService : IReservaSalaService
    {
        private readonly IReservaSalaService _service;
        private static ReservaValidator validatorReserva = new ReservaValidator();

        private List<Sala> Salas { get; set; }
        private HistoricoReserva HistoricoReserva { get; set; }

        public ReservaSalaService(IReservaSalaService service)
        {
            _service = service;
        }


        public async Task<string> ReservarSalas(string[] entradaDados)
        {
            CarregarSalas();
            HistoricoReserva = new HistoricoReserva()
            {
                InformacoesReservas = new List<string>(),
                Reservas = new List<Reserva>()
            };

            foreach (var item in entradaDados)
            {
                await EfetuarReservaAsync(item.Split(";"));
            }

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

        private async Task EfetuarReservaAsync(string[] dados)
        {
            Reserva tentativaReserva = new Reserva
            {
                DataInicio = DateTime.Parse(string.Concat(dados[0], " ", dados[1])),
                DataFim = DateTime.Parse(string.Concat(dados[2], " ", dados[3])),
                QuantidadePessoas = Convert.ToInt32(dados[4]),
                Sala = new Sala()
            };

            bool possuiInternet = dados[5].Equals("Sim") ? true : false;
            bool possuiTvWebcam = dados[6].Equals("Sim") ? true : false;

            var salaDisponivel = VerificarSalaDisponivel(tentativaReserva, possuiInternet, possuiTvWebcam);

            tentativaReserva.Sala = salaDisponivel;

            var validador = await validatorReserva.ValidateAsync(tentativaReserva);

            string informacoesReserva = (validador.IsValid) ?
                "ok" : string.Concat(validador.Errors.Select(x => x.ErrorMessage).ToList());

            HistoricoReserva.Reservas.Add(tentativaReserva);
            HistoricoReserva.InformacoesReservas.Add(informacoesReserva);
        }

        private Sala VerificarSalaDisponivel(Reserva reserva, bool possuiInternet, bool possuiTvWebcam)
        {
            var salasAtendem = Salas.Where(s =>
                s.Capacidade >= reserva.QuantidadePessoas.Value &&
                s.PossuiInternet.Equals(possuiInternet) &&
                s.PossuiTvWebcam.Equals(possuiTvWebcam)).ToList();

            foreach (var sala in salasAtendem)
            {
                if (HistoricoReserva.Reservas == null)
                    return sala;
                else if (!HistoricoReserva.Reservas.Contains(reserva))
                    return sala;
            }

            return null;
        }
    }
}
