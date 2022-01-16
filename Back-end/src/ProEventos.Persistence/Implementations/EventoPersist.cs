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
    public class EventoPersist : BasePersist<Evento>, IEventoPersist
    {
        public EventoPersist(ProEventosContext context) : base(context)
        {
            //Desabilita o rastreamento nesse contexto
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public async Task<Evento> GetEventosByIdAsync(int id, bool includePalestrantes)
        {
            IQueryable<Evento> query = _dataset
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(e => e.PalestrantesEventos) //Para cada Palestrante que existir
                .ThenInclude(p => p.Palestrante); //Inclua na query
            }

            query = query
            .OrderBy(e => e.Id)
            .Where(e => e.Id.Equals(id));

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes)
        {
            IQueryable<Evento> query = _dataset
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(e => e.PalestrantesEventos) //Para cada Palestrante que existir
                .ThenInclude(p => p.Palestrante); //Inclua na query
            }

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _dataset
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if (includePalestrantes)
            {
                query = query
                .Include(e => e.PalestrantesEventos) //Para cada Palestrante que existir
                .ThenInclude(p => p.Palestrante); //Inclua na query
            }

            query = query
            .OrderBy(e => e.Id)
            .Where(e => e.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }


    }
}
