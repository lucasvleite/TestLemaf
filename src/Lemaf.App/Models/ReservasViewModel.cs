using System;

namespace Lemaf.App.Models
{
    public class ReservasViewModel
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public int QuantidadePessoas { get; set; }

        public int CodigoSala { get; set; }
        public int Capacidade { get; set; }
        public bool PossuiComputador { get; set; }
        public bool PossuiInternet { get; set; }
        public bool PossuiTvWebcam { get; set; }

        public string Informações { get; set; }
    }
}
