using Microsoft.EntityFrameworkCore;
using Teste.Api.Data.Mappings;
using Teste.Api.Models;

namespace Teste.Api.Data
{
    public class TesteDbContext : DbContext
    {
        public TesteDbContext(DbContextOptions<TesteDbContext> options) : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMap());
        }
    }
}
