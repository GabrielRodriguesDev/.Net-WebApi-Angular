using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector.Logging;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Context
{
    public class ProEventosContext : DbContext
    {
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) { }


        public DbSet<Evento> Eventos { get; set; }

        public DbSet<Palestrante> Palestrantes { get; set; }

        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }

        public DbSet<Lote> Lotes { get; set; }

        public DbSet<RedeSocial> RedesSociais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<PalestranteEvento>() // Configurando tabela com relacionamento muitos para muitos
                .HasKey(PE => new { PE.EventoId, PE.PalestranteId });

            modelBuilder.Entity<Evento>() // Entidade Evento
            .HasMany(e => e.RedesSociais) // Evento tem "muitas redes sociais"
            .WithOne(rs => rs.Evento)  // Uma rede social pode pertercer a um evento
            .OnDelete(DeleteBehavior.Cascade); // Delete Cascade (Deletou um evento que possui uma rede social, deleta a rede também.)

            modelBuilder.Entity<Palestrante>() // Entidade Palestrante
                .HasMany(e => e.RedesSociais)   // Possui muitas redes Sociais
                .WithOne(rs => rs.Palestrante) // Uma rede só pode ter um palestrante
                .OnDelete(DeleteBehavior.Cascade); // Ao deletar um palestrante apagar também suas redes sociais.
        }
    }
}
