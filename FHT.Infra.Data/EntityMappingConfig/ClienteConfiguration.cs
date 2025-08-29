using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> cliente)
        {
            cliente.ToTable("CLIENTE");

            cliente.HasKey(x => x.ClienteId)
                   .HasName("PK_CLIENTE");

            cliente.Property(x => x.ClienteId)
                   .HasColumnName("CLIENTE_ID")
                   .UseIdentityColumn();

            cliente.Property(x => x.Tipo)
                   .HasColumnName("TIPO_CLIENTE")
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsRequired();

            cliente.Property(x => x.Status)
                   .HasColumnName("STATUS_CLIENTE")
                   .HasConversion<string>()
                   .HasMaxLength(10)
                   .IsRequired();

            cliente.Property(x => x.Nome)
                   .HasColumnName("NOME")
                   .HasMaxLength(200)
                   .IsRequired();

            cliente.Property(x => x.Saldo)
                   .HasColumnName("SALDO")
                   .HasColumnType("decimal(18,2)");

            cliente.Property(x => x.DataCadastro)
                   .HasColumnName("DATA_CADASTRO")
                   .HasColumnType("datetimeoffset(6)")
                   .IsRequired();

            cliente.Property(x => x.DataAtualizacao)
                   .HasColumnName("DATA_ATUALIZACAO")
                   .HasColumnType("datetimeoffset(6)")
                   .IsRequired();

            cliente.HasMany(x => x.Contas)
                   .WithOne(c => c.Cliente)
                   .HasForeignKey(c => c.ClienteId)
                   .HasConstraintName("FK_CONTA_CLIENTE")
                   .OnDelete(DeleteBehavior.Restrict);

            cliente.HasMany(x => x.Compliance)
                   .WithOne(c => c.Cliente)
                   .HasForeignKey(c => c.ClienteId)
                   .HasConstraintName("FK_COMPLIANCE_CLIENTE")
                   .OnDelete(DeleteBehavior.Cascade);

            cliente.HasMany(x => x.Contato)
                   .WithOne(c => c.Cliente)
                   .HasForeignKey(c => c.ClienteId)
                   .HasConstraintName("FK_CONTATO_CLIENTE")
                   .OnDelete(DeleteBehavior.Cascade);

            cliente.HasOne(x => x.DadosPessoais)
                   .WithOne(dp => dp.Cliente)
                   .HasForeignKey<DadoPessoal>(dp => dp.ClienteId)
                   .HasConstraintName("FK_DADO_PESSOAL_CLIENTE")
                   .OnDelete(DeleteBehavior.Cascade);

            cliente.HasMany(x => x.DocumentosFiscais)
                   .WithOne(c => c.Cliente)
                   .HasForeignKey(c => c.ClienteId)
                   .HasConstraintName("FK_DOCUMENTO_FISCAL_CLIENTE")
                   .OnDelete(DeleteBehavior.Cascade);

            cliente.HasMany(x => x.Endereco)
                   .WithOne(c => c.Cliente)
                   .HasForeignKey(c => c.ClienteId)
                   .HasConstraintName("FK_ENDERECO_FISCAL_CLIENTE")
                   .OnDelete(DeleteBehavior.Cascade);

            cliente.HasMany(x => x.Societario)
                   .WithOne(c => c.Cliente)
                   .HasForeignKey(c => c.ClienteId)
                   .HasConstraintName("FK_SOCIETARIO_CLIENTE")
                   .OnDelete(DeleteBehavior.Cascade);

            cliente.HasIndex(x => new { x.Status, x.Tipo })
                   .HasDatabaseName("IX_CLIENTE_STATUS_TIPO");

            cliente.HasIndex(x => x.Nome)
                   .HasDatabaseName("IX_CLIENTE_NOME");
        }
    }
}
