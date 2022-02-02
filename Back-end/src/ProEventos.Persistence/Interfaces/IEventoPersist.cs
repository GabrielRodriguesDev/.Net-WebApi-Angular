using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Interfaces
{
    public interface IEventoPersist : IBasePersist<Evento>
    {


        /// <summary>
        /// Método get que retornará apenas 1 evento
        /// </summary>
        /// <param name="id">Código chave da tabela Evento</param>
        /// <param name="includePalestrantes">Condicional para incluir os palestrantes ao evento</param>
        /// <returns>Apenas 1 Evento</returns>
        Task<Evento> GetEventosByIdAsync(int id, bool includePalestrantes = false);

        /// <summary>
        /// Método get que retornará uma lista de eventos com base no tema.
        /// </summary>
        /// <param name="tema">Atributo tema do objeto e campo da tabela Evento</param>
        /// <param name="includePalestrantes">Condicional para incluir os palestrantes ao evento</param>
        /// <returns>Lista de Evento</returns>
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);

        /// <summary>
        /// Método get que retornará uma lista de eventos.
        /// </summary>
        /// <param name="includePalestrantes">Condicional para incluir os palestrantes ao evento</param>
        /// <returns>Lista de Evento</returns>
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);

    }
}
