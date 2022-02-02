using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Implementations;

namespace ProEventos.Persistence.Interfaces
{
    public interface IBasePersist<T> where T : class
    {


        void Add(T item);

        void Update(T item);

        void Delete(T item);

        Task<bool> SaveChangesAsync();

    }
}
