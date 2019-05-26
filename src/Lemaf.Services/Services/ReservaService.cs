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
        private const string Ok = "ok";
        private const string Sim = "Sim";

        private static HistoricoReserva HistoricoReserva =
            new HistoricoReserva()
            {
                InformacoesReservas = new List<string>(),
                Reservas = new List<Reserva>()
            };

        public async Task<string> ReservarSalas(string[] entradaDados)
        {
            foreach (string item in entradaDados)
            {
                string verificador = VerificarDados(item);

                if (verificador.Equals(Ok))
                    await EfetuarReservaAsync(item.Split(";"));
                else
                    AdicionarHistoricoReserva(verificador, null);
            }

            return JsonConvert.SerializeObject(HistoricoReserva);
        }

        private string VerificarDados(string entrada)
        {
            string[] dados = entrada.Split(";");
            try
            {
                DateTime dataInicio = DateTime.Parse(string.Concat(dados[0], " ", dados[1]));
                DateTime dataFinal = DateTime.Parse(string.Concat(dados[2], " ", dados[3]));
                int quantidadePessoas = Convert.ToInt32(dados[4]);
                bool internet = dados[5].Equals(Sim) ? true : false;
                bool tvWebcam = dados[6].Equals(Sim) ? true : false;
            } catch(Exception ex)
            {
                return ex.Message;
            }
            
            return Ok;
        }


        private async Task EfetuarReservaAsync(string[] dados)
        {
            Reserva tentativaReserva = new Reserva
            {
                DataInicio = DateTime.Parse(string.Concat(dados[0], " ", dados[1])),
                DataFinal = DateTime.Parse(string.Concat(dados[2], " ", dados[3])),
                QuantidadePessoas = Convert.ToInt32(dados[4]),
                Sala = null
            };

            string informacoesReserva = await VerificarRegras(tentativaReserva);

            if(informacoesReserva == Ok){
                tentativaReserva.Sala = VerificarSalaDisponivel(
                    tentativaReserva,
                    dados[5].Equals(Sim) ? true : false,
                    dados[6].Equals(Sim) ? true : false);
                
                if(tentativaReserva.Sala == null)
                    AdicionarHistoricoReserva(informacoesReserva, null);
                else
                    AdicionarHistoricoReserva(informacoesReserva, tentativaReserva);
            }
            else
            {
                AdicionarHistoricoReserva(informacoesReserva, null);
            }
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
                VerificarSalasAtendemNecessidade(copiaReserva.QuantidadePessoas, possuiInternet, possuiTvWebcam);

            foreach (var sala in salasAtendem)
            {
                copiaReserva.Sala = sala;
                 if ( !ConflitoHorario(copiaReserva) )
                    return sala;
            }
            return null;
        }

        //TODO: Arrumar erros - verificar tres outras salas.
        private async Task<string> VerificarRegras(Reserva reserva)
        {
            var validador = await validator.ValidateAsync(reserva);

            if(!validador.IsValid)
            {
                var erro = string.Concat(validador.Errors.Select(x => x.ErrorMessage).ToList());
                return erro;
            }
            
            return Ok;
        }

        private bool ConflitoHorario(Reserva reserva)
        {
            var verificador = HistoricoReserva.Reservas
                .Where(x => x.Sala.CodigoSala == reserva.Sala.CodigoSala & (x.DataInicio > reserva.DataFinal | reserva.DataInicio > x.DataFinal)
            ).ToList();

            return (verificador.Count.Equals(0)) ? false : true;
        }

        private static Reserva NovaReserva(DateTime dataInicio, DateTime dataFinal, int quantidadePessoas, Sala sala) =>
            new Reserva
            {
                DataInicio = dataInicio,
                DataFinal = dataFinal,
                QuantidadePessoas = quantidadePessoas,
                Sala = sala
            };
    }
}
