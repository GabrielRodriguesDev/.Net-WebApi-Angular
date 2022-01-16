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
    public class PalestrantePersist : BasePersist<Palestrante>, IPalestrantePersist
    {
        public PalestrantePersist(ProEventosContext context) : base(context)
        {
            //Desabilita o rastreamento nesse contexto
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }


        public async Task<Palestrante> GetPalestrantesByIdAsync(int id, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(p => p.Id).Where(p => p.Id.Equals(id));

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
            .Include(p => p.RedesSociais);

            if (includeEventos)
            {
                query = query.Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(p => p.Id)
            .Where(p => p.Nome.ToLower()
            .Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(p => p.RedesSociais);

            if (includeEventos)
            {
                query = query
                .Include(p => p.PalestrantesEventos)
                .ThenInclude(pe => pe.Evento);
            }

            query = query.OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }
    }
}
