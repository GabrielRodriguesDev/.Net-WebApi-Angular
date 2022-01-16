using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Interfaces
{
    public interface IEventoPersist : IBasePersist<Evento>
    {
        #region  Eventos
        Task<Evento> GetEventosByIdAsync(int id, bool includePalestrantes = false);

        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);

        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);

        #endregion
    }
}
