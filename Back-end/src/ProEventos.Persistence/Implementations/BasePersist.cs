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
    public class BasePersist<T> : IBasePersist<T> where T : class
    {

        protected readonly ProEventosContext _context;
        protected DbSet<T> _dataset;
        public BasePersist(ProEventosContext context)
        {
            _context = context;
            _dataset = context.Set<T>();
        }

        #region Base
        public void Add(T item)
        {
            _dataset.Add(item);
        }

        public void Update(T item)
        {
            _dataset.Update(item);
        }

        public void Delete(T item)
        {
            _dataset.Remove(item);
        }

        public void DeleteRange(T[] itemArray)
        {
            _context.RemoveRange(itemArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                var teste = await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var testeTrhow = ex.Message;
                return false;
            }
        }

        #endregion

    }
}
