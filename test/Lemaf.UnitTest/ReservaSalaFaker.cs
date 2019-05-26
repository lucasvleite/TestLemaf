using Bogus;
using Lemaf.Entities;
using Lemaf.Services.Services;
using Lemaf.Services.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lemaf.UnitTest
{
    public class ReservaSalaFaker
    {
        private static ReservaValidator validator = new ReservaValidator();

        public static readonly string[] EntradaDadosErroDataInicial =
        {
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2018;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-99-2019;10:00;26-05-2019;12:00;10;Sim;Não"
        };

        public static readonly string[] EntradaDadosErroDataFinal =
        {
            "26-05-2019;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-05-2018;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-99-2019;12:00;10;Sim;Não"
        };
    }
}
