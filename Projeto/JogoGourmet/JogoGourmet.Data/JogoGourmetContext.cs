using JogoGourmet.Core.Data;
using JogoGourmet.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoGourmet.Data
{
    public class JogoGourmetContext : DbContext, IUnitOfWork
    {
        //public JogoGourmetContext(DbContextOptions<JogoGourmetContext> options): base(options) 
        //{
         
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("inMemoryDb");
        }

        public DbSet<Prato> Pratos { get; set; }

        public DbSet<ArvorePrato> ArvorePratos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var property in modelbuilder.Model.GetEntityTypes()
                    .SelectMany(
                        e => e.GetProperties().Where(p => p.ClrType == typeof(string))
                        )
                )
                    property.SetColumnType("varchar(100)");/*não usar nvarchar max*/
            {
                modelbuilder.ApplyConfigurationsFromAssembly(typeof(JogoGourmetContext).Assembly);
            }
        }
        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
