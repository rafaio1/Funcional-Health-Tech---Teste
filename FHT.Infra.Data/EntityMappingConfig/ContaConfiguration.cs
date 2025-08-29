using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class ContaConfiguration : IEntityTypeConfiguration<Conta>
    {
        public void Configure(EntityTypeBuilder<Conta> conta)
        {
            conta.ToTable("CONTA");

            conta.HasKey(x => x.ContaId)
                 .HasName("PK_CONTA");

            conta.Property(x => x.ContaId)
                 .HasColumnName("CONTA_ID")
                 .UseIdentityColumn();

            conta.Property(x => x.ClienteId)
                 .HasColumnName("CLIENTE_ID")
                 .IsRequired();

            conta.Property(x => x.Tipo)
                 .HasColumnName("TIPO_CONTA")
                 .HasConversion<string>()
                 .HasMaxLength(20)
                 .IsRequired();

            conta.Property(x => x.Status)
                 .HasColumnName("STATUS_CONTA")
                 .HasConversion<string>()
                 .HasMaxLength(20)
                 .IsRequired();

            conta.Property(x => x.Agencia)
                 .HasColumnName("AGENCIA")
                 .HasMaxLength(10)
                 .IsRequired();

            conta.Property(x => x.Numero)
                 .HasColumnName("NUMERO")
                 .HasMaxLength(20)
                 .IsRequired();

            conta.Property(x => x.Digito)
                 .HasColumnName("DIGITO")
                 .HasMaxLength(4);

            conta.Property(x => x.Saldo)
                 .HasColumnName("SALDO")
                 .HasColumnType("decimal(18,2)")
                 .HasDefaultValue(0m);

            conta.Property(x => x.DataAbertura)
                 .HasColumnName("DATA_ABERTURA")
                 .HasColumnType("datetimeoffset(6)")
                 .IsRequired();

            conta.Property(x => x.DataEncerramento)
                 .HasColumnName("DATA_ENCERRAMENTO")
                 .HasColumnType("datetimeoffset(6)");

            conta.HasIndex(x => x.ClienteId)
                 .HasDatabaseName("IX_CONTA_CLIENTE");

            conta.HasIndex(x => new { x.Agencia, x.Numero, x.Digito })
                 .HasDatabaseName("UX_CONTA_BANCO")
                 .IsUnique();
        }
    }
}
