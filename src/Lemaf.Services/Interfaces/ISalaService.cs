using Lemaf.Entities;
using System.Collections.Generic;

namespace Lemaf.Services.Interfaces
{
    public interface ISalaService
    {
        List<Sala> VerificarSalasAtendemNecessidade(int quantidadePessoas, bool possuiInternet, bool possuiTvWebcam);
    }
}
