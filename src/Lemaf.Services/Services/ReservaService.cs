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
    public class ReservaService : IReservaService
    {
        private static ReservaValidator validator = new ReservaValidator();

        private HistoricoReserva HistoricoReserva { get; set; }
        

        public async Task<string> ReservarSalas(string[] entradaDados)
        {
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

        private async Task EfetuarReservaAsync(string[] dados)
        {
            Reserva tentativaReserva = new Reserva
            {
                DataInicio = DateTime.Parse(string.Concat(dados[0], " ", dados[1])),
                DataFim = DateTime.Parse(string.Concat(dados[2], " ", dados[3])),
                QuantidadePessoas = Convert.ToInt32(dados[4]),
                Sala = new Sala()
            };

            tentativaReserva.Sala = VerificarSalaDisponivel(
                tentativaReserva,
                dados[5].Equals("Sim") ? true : false,
                dados[6].Equals("Sim") ? true : false);

            var validador = await validator.ValidateAsync(tentativaReserva);
            string informacoesReserva = (validador.IsValid) ?
                "ok" : string.Concat(validador.Errors.Select(x => x.ErrorMessage).ToList());

            AdicionarHistoricoReserva(informacoesReserva, tentativaReserva);
        }

        private void AdicionarHistoricoReserva(string informacoesReserva, Reserva tentativaReserva)
        {
            HistoricoReserva.InformacoesReservas.Add(informacoesReserva);
            HistoricoReserva.Reservas.Add(tentativaReserva);
        }

        private Sala VerificarSalaDisponivel(Reserva reserva, bool possuiInternet, bool possuiTvWebcam)
        {
            var copiaReserva = reserva;
            var salasAtendem = new SalaService().
                VerificarSalasAtendemNecessidade(copiaReserva.QuantidadePessoas.Value, possuiInternet, possuiTvWebcam);

            foreach (var sala in salasAtendem)
            {
                copiaReserva.Sala = sala;
                if (HistoricoReserva.Reservas == null)
                    return sala;
                else if (!HistoricoReserva.Reservas.Contains(copiaReserva))
                    return sala;
            }
            return null;
        }

        private static Reserva NovaReserva(DateTime dataInicio, DateTime dataFinal, int quantidadePessoas, Sala sala) =>
            new Reserva
            {
                DataInicio = dataInicio,
                DataFim = dataFinal,
                QuantidadePessoas = quantidadePessoas,
                Sala = sala
            };
    }
}
