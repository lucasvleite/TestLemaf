using System;

namespace Lemaf.Entities
{
    public class Reserva
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public int QuantidadePessoas { get; set; }
        public Sala Sala { get; set; }
    }
}
