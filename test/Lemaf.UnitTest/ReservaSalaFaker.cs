﻿namespace Lemaf.UnitTest
{
    public class ReservaSalaFaker
    {
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

        public static readonly string[] EntradaDadosMista =
        {
            "18-06-2019;08:57;18-06-2019;10:18;10;Sim;Sim",
            "26-05-2018;10:00;26-05-2019;12:00;10;Sim;Sim",
            "26-99-2019;10:00;26-05-2019;12:00;10;Sim;Não",
            "03-07-2019;21:19;04-07-2019;04:07;6;Sim;Sim",
            "10-06-2019;23:27;11-06-2019;02:53;9;Sim;Sim",
            "26-05-2019;10:00;26-05-2018;12:00;10;Sim;Sim",
            "26-05-2019;10:00;26-99-2019;12:00;10;Sim;Não",
            "28-06-2019;21:21;29-06-2019;05:12;7;Sim;Sim",
            "30-06-2019;13:39;30-06-2019;20:05;10;Sim;Sim",
            "30-05-2019;06:05;30-05-2019;14:04;7;Sim;Não",
            "09-06-2019;02:38;09-06-2019;04:41;3;Sim;Não",
            "30-06-2019;03:44;30-06-2019;11:08;3;Sim;Sim"
        };
    }
}
