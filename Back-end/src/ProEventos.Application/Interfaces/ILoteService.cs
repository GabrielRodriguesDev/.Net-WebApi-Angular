using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Interfaces
{
    public interface ILoteService
    {

        /// <summary>
        /// Método que vai salvar um lote
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <param name="models">Lista de objetos LotesDto</param>
        /// <returns>Lista de LoteDto</returns>
        Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models);

        /// <summary>
        /// Método que vai deletar um lote de um evento
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <param name="loteId">Código chave da tabela lote</param>
        /// <returns>Uma condicional</returns>
        Task<bool> DeleteLote(int eventoId, int loteId);

        /// <summary>
        /// Método que retornará uma lista de lotes por um evento Id
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <returns>Lista de LoteDto</returns>
        Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId);

        /// <summary>
        /// Método get que retornará apenas 1 Lote
        /// </summary>
        /// <param name="eventoId">Código chave da tabela Evento</param>
        /// <param name="loteId">Código chave da tabela lote</param>
        /// <returns>Apenas 1 LoteDto</returns>
        Task<LoteDto> GetLoteByIdAsync(int eventoId, int loteId);
    }
}
