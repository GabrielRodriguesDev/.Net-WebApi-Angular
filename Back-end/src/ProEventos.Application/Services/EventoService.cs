using System;
using System.Threading.Tasks;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private IEventoPersist _persist;
        public EventoService(IEventoPersist persist)
        {
            _persist = persist;
        }
        public async Task<Evento> AddEventos(Evento model)
        {
            try
            {
                _persist.Add(model);
                if (await _persist.SaveChangesAsync())
                {
                    return await GetEventosByIdAsync(model.Id);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEvento(Evento model)
        {
            try
            {
                var evento = await _persist.GetEventosByIdAsync(model.Id);

                if (evento == null) return null;

                _persist.Update(model);
                if (await _persist.SaveChangesAsync())
                {
                    return await _persist.GetEventosByIdAsync(model.Id);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int id)
        {
            try
            {
                var evento = await _persist.GetEventosByIdAsync(id);
                if (evento == null) throw new Exception("Evento para delete não foi encontrado");

                _persist.Delete(evento);
                return await _persist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> GetEventosByIdAsync(int id, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _persist.GetEventosByIdAsync(id, includePalestrantes);
                if (evento == null) return null;

                return evento;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _persist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _persist.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
