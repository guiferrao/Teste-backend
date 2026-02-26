using Microsoft.EntityFrameworkCore;
using Teste.Api.Models;

namespace Teste.Api.Data
{
    public class TesteDbContext : DbContext
    {
        public TesteDbContext(DbContextOptions<TesteDbContext> options) : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
