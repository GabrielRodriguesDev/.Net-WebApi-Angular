using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Interfaces
{
    public interface IEventoService
    {
        Task<EventoDto> AddEventos(EventoDto model);

        Task<EventoDto> UpdateEvento(EventoDto model);

        Task<bool> DeleteEvento(int id);

        Task<EventoDto> GetEventoByIdAsync(int id, bool includePalestrantes = false);

        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);

        Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false);
    }
}
