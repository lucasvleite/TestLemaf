using Lemaf.Entities;
using Lemaf.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Lemaf.Services.Services
{
    public class SalaService : ISalaService
    {
        private static List<Sala> Salas
        {
            get
            {
                var salas = new List<Sala>();
                for (int i = 1; i <= 5; i++)
                {
                    salas.Add(NovaSala(i, 10, true, true, true));
                }

                salas.Add(NovaSala(6, 10, false, true, false));
                salas.Add(NovaSala(7, 10, false, true, false));

                salas.Add(NovaSala(8, 3, true, true, true));
                salas.Add(NovaSala(9, 3, true, true, true));
                salas.Add(NovaSala(10, 3, true, true, true));

                salas.Add(NovaSala(11, 20, false, false, false));
                salas.Add(NovaSala(12, 20, false, false, false));

                return salas;
            }
        }

        public List<Sala> GetSalas() => Salas;

        private static Sala NovaSala(int codigoSala, int capacidade, bool possuiComputador, bool possuiInternet, bool possuiTvWebcam) =>
            new Sala
            {
                CodigoSala = codigoSala,
                Capacidade = capacidade,
                PossuiComputador = possuiComputador,
                PossuiInternet = possuiInternet,
                PossuiTvWebcam = possuiTvWebcam
            };

        public List<Sala> VerificarSalasAtendemNecessidade(int quantidadePessoas, bool possuiInternet, bool possuiTvWebcam)
        {
            var salasAtendem = Salas.Where(s =>
                s.Capacidade >= quantidadePessoas &&
                s.PossuiInternet.Equals(possuiInternet) &&
                s.PossuiTvWebcam.Equals(possuiTvWebcam)).ToList();

            return salasAtendem;
        }
    }
}
