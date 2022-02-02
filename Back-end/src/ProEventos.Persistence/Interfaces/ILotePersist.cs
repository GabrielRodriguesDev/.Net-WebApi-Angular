using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Interfaces
{
    public interface ILotePersist : IBasePersist<Lote>
    {
        /// <summary>
        /// Método que retornará uma lista de lotes por um evento Id
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <returns>Lista de Lotes</returns>
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);

        /// <summary>
        /// Método get que retornará apenas 1 Lote
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <param name="loteId">Código chave da tabela lote</param>
        /// <returns>Apenas 1 Lote</returns>
        Task<Lote> GetLoteByIdAsync(int eventoId, int loteId);


    }
}
