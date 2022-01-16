using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProEventos.Persistence.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<ProEventosContext>
    {
        public ProEventosContext CreateDbContext(string[] args)
        {
            var connectionString = "server=localhost;port=3306;database=ProEventos;uid=root;password=fx870";

            var optionsBuilder = new DbContextOptionsBuilder<ProEventosContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new ProEventosContext(optionsBuilder.Options);
        }
    }
}
