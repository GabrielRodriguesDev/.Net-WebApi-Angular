using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Interfaces
{
    public interface IPalestrantePersist : IBasePersist<Palestrante>
    {
        #region  Palestrantes
        Task<Palestrante> GetPalestrantesByIdAsync(int id, bool includeEventos = false);

        Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos = false);

        Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false);

        #endregion
    }
}
