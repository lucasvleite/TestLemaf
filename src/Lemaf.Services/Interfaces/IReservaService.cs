using System.Threading.Tasks;

namespace Lemaf.Services.Interfaces
{
    public interface IReservaService
    {
        Task<string> ReservarSalas(string[] entradaDados);
    }
}
