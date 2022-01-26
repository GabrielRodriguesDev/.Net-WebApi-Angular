using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Services
{
    public class EventoService : IEventoService
    {
        private IEventoPersist _persist;

        private IMapper _mapper;
        public EventoService(IEventoPersist persist, IMapper mapper)
        {
            _persist = persist;
            _mapper = mapper;
        }
        public async Task<EventoDto> AddEventos(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _persist.Add(evento);
                if (await _persist.SaveChangesAsync())
                {
                    return await GetEventosByIdAsync(evento.Id);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(EventoDto model)
        {
            try
            {


                var evento = await _persist.GetEventosByIdAsync(model.Id);
                if (evento == null) return null;

                _mapper.Map(model, evento);
                _persist.Update(evento);
                if (await _persist.SaveChangesAsync())
                {
                    var retorno = await _persist.GetEventosByIdAsync(evento.Id);
                    return _mapper.Map<EventoDto>(retorno);
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

        public async Task<EventoDto> GetEventosByIdAsync(int id, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _persist.GetEventosByIdAsync(id, includePalestrantes);
                if (evento == null) return null;

                return _mapper.Map<EventoDto>(evento);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _persist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                return _mapper.Map<EventoDto[]>(eventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _persist.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                return _mapper.Map<EventoDto[]>(eventos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
