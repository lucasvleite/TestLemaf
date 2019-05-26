using System.Threading.Tasks;
using Lemaf.Entities;

namespace Lemaf.Services.Interfaces
{
    public interface IReservaService
    {
        Task<string> ReservarSalas(string[] entradaDados);
    }
}
