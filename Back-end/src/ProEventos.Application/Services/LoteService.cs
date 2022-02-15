using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Application.Services
{
    public class LoteService : ILoteService
    {
        private readonly ILotePersist _persist;

        
        private readonly IMapper _mapper;



        public LoteService(ILotePersist persist, IMapper mapper)
        {
            _persist = persist;
            _mapper = mapper;
            
        }


        public async Task<LoteDto> GetLoteByIdAsync(int eventoId, int loteId)
        {
            try
            {
                var entity = await _persist.GetLoteByIdAsync(eventoId, loteId);
                if (entity == null) return null;

                return _mapper.Map<LoteDto>(entity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var entity = await _persist.GetLotesByEventoIdAsync(eventoId);
                if (entity == null) return null;

                return _mapper.Map<LoteDto[]>(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var entity = await _persist.GetLoteByIdAsync(eventoId, loteId);
                if (entity == null) return false;

                _persist.Delete(entity);
                return await _persist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> Addlote(int eventoId, LoteDto model)
        {
            try
            {
                model.EventoId = eventoId;
                var entity = _mapper.Map<Lote>(model);
                
                _persist.Add(entity);
                return await _persist.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void UpdateLote(Lote[] listEntity, LoteDto model)
        {
            try
            {
                var entity = listEntity.FirstOrDefault(entity => entity.Id == model.Id);
                _mapper.Map(model, entity);
                _persist.Update(entity);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLote(int eventoId, LoteDto[] models)
        {
            try
            {
                var listEntity = await _persist.GetLotesByEventoIdAsync(eventoId);

                if (listEntity == null) return null;

                foreach (var model in models)
                {
                    if (model.Id == 0)
                    {
                        if (!await Addlote(eventoId, model))
                        {
                            return null;
                        }
                    }
                    else
                    {   
                        model.EventoId = eventoId;
                        UpdateLote(listEntity, model);
                    }
                }
                if (await _persist.SaveChangesAsync())
                {
                    var listResult = await _persist.GetLotesByEventoIdAsync(eventoId);
                    return _mapper.Map<LoteDto[]>(listResult);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
