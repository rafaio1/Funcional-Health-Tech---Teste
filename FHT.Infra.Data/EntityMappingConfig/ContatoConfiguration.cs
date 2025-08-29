using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class ContatoConfiguration : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> contato)
        {
            contato.ToTable("CONTATO");

            contato.HasKey(x => x.ContatoId)
                   .HasName("PK_CONTATO");

            contato.Property(x => x.ContatoId)
                   .HasColumnName("CONTATO_ID")
                   .UseIdentityColumn(); 

            contato.Property(x => x.ClienteId)
                   .HasColumnName("CLIENTE_ID")
                   .IsRequired();

            contato.Property(x => x.Tipo)
                   .HasColumnName("TIPO")
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            contato.Property(x => x.Valor)
                   .HasColumnName("VALOR")
                   .HasMaxLength(200)
                   .IsRequired();

            contato.Property(x => x.Principal)
                   .HasColumnName("PRINCIPAL")
                   .HasColumnType("bit")
                   .IsRequired();

            contato.Property(x => x.Observacao)
                   .HasColumnName("OBSERVACAO")
                   .HasMaxLength(500);

            contato.Property(x => x.DataCadastro)
                   .HasColumnName("DATA_CADASTRO")
                   .HasColumnType("datetimeoffset(6)")
                   .IsRequired();

            contato.Property(x => x.DataAtualizacao)
                   .HasColumnName("DATA_ATUALIZACAO")
                   .HasColumnType("datetimeoffset(6)")
                   .IsRequired();

            contato.HasIndex(x => new { x.ClienteId, x.Tipo, x.Principal })
                   .HasDatabaseName("IX_CONTATO_CLIENTE_TIPO_PRINC");
        }
    }
}
