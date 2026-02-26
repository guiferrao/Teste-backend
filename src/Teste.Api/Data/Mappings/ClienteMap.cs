using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teste.Api.Models;

namespace Teste.Api.Data.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<Models.Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(c => c.Email)
                .IsRequired();

            builder.HasIndex(c => c.Email)
                .IsUnique();

            builder.Property(c => c.Telefone)
                .IsRequired();

            builder.HasIndex(c => c.Telefone)
                .IsUnique();

            builder.Property(c => c.Cep)
                .IsRequired();

            builder.Property(c => c.Numero)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.Complemento)
                .HasMaxLength(60);

            builder.Property(c => c.Uf)
                .HasMaxLength(2);
        }
    }
}
