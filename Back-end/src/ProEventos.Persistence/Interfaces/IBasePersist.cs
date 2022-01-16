using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Interfaces
{
    public interface IBasePersist<T> where T : class
    {

        #region Base
        void Add(T item);

        void Update(T item);

        void Delete(T item);

        void DeleteRange(T[] item);

        Task<bool> SaveChangesAsync();

        #endregion

    }
}
