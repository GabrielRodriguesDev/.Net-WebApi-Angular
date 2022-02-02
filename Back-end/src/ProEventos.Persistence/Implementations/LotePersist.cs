using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Interfaces;

namespace ProEventos.Persistence.Implementations
{
    public class LotePersist : BasePersist<Lote>, ILotePersist
    {


        public LotePersist(ProEventosContext context) : base(context)
        { }
        public async Task<Lote> GetLoteByIdAsync(int eventoId, int loteId)
        {
            IQueryable<Lote> query = _dataset.AsNoTracking()
                                        .Where(lote => lote.EventoId == eventoId
                                                    && lote.Id == loteId);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _dataset.AsNoTracking()
                                        .Where(lote => lote.EventoId == eventoId);
            return await query.ToArrayAsync();
        }

        public async Task<bool> DeleteLote(Lote lote)
        {
            _dataset.Remove(lote);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
