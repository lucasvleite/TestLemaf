using System.Threading.Tasks;

namespace Lemaf.Services.Interfaces
{
    public interface IReservaSalaService
    {
        Task<string> ReservarSalas(string[] entradaDados);
    }
}
