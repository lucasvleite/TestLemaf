using Lemaf.App.Models;
using Lemaf.Entities;
using Lemaf.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lemaf.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Reservas(IFormCollection dadosFormulario)
        {
            if (dadosFormulario.Files.Count.Equals(0) | dadosFormulario.Files == null)
            {
                ViewData["Erro"] = "Arquivo não selecionado!";
                return View("Index");
            }

            var arquivo = dadosFormulario.Files.First();

            if (!arquivo.FileName.Contains(".txt"))
            {
                ViewData["Erro"] = "Necessário enviar um arquivo texto .txt!";
                return View("Index");
            }

            var reservas = await EfetuarReservas(arquivo);

            return View(PreencherModel(reservas));
        }

        private static async Task<string> EfetuarReservas(IFormFile arquivo)
        {
            string pastaTemporaria = Path.GetTempFileName();

            using (var stream = new FileStream(pastaTemporaria, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            string[] leituraArquivo = System.IO.File.ReadAllLines(pastaTemporaria);

            return await new ReservaService().ReservarSalasAsync(leituraArquivo);
        }

        private static List<ReservasViewModel> PreencherModel(string resultado)
        {
            var json = JsonConvert.DeserializeObject<HistoricoReserva>(resultado);
            List<ReservasViewModel> reservas = new List<ReservasViewModel>();

            int i = 0;
            foreach (var item in json.Reservas)
            {
                reservas.Add(new ReservasViewModel
                {
                    DataInicio = item.DataInicio,
                    DataFinal = item.DataFinal,
                    QuantidadePessoas = item.QuantidadePessoas,
                    CodigoSala = item.Sala == null ? 0 : item.Sala.CodigoSala,
                    Capacidade = item.Sala == null ? 0 : item.Sala.Capacidade,
                    PossuiComputador = item.Sala == null ? false : item.Sala.PossuiComputador,
                    PossuiInternet = item.Sala == null ? false : item.Sala.PossuiInternet,
                    PossuiTvWebcam = item.Sala == null ? false : item.Sala.PossuiTvWebcam,
                    Informações = json.InformacoesReservas.ElementAt(i)
                });

                i++;
            }

            return reservas;
        }
    }
}
