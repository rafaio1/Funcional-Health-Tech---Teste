using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class ComprovanteConfiguration : IEntityTypeConfiguration<Comprovante>
    {
        public void Configure(EntityTypeBuilder<Comprovante> comprovante)
        {
            comprovante.ToTable("COMPROVANTE");

            comprovante.HasKey(x => x.ComprovanteId)
             .HasName("PK_COMPROVANTE");

            comprovante.Property(x => x.ComprovanteId)
             .HasColumnName("COMPROVANTE_ID")
             .UseIdentityColumn(); 

            comprovante.Property(x => x.CobrancaId)
             .HasColumnName("COBRANCA_ID")
             .IsRequired();

            comprovante.Property(x => x.ClienteId)
             .HasColumnName("CLIENTE_ID")
             .IsRequired();

            comprovante.Property(x => x.Metodo)
             .HasColumnName("METODO")
             .HasConversion<string>()
             .HasMaxLength(20)
             .IsRequired();

            comprovante.Property(x => x.SituacaoNoMomento)
             .HasColumnName("SITUACAO_MOMENTO")
             .HasConversion<string>()
             .HasMaxLength(20)
             .IsRequired();

            comprovante.Property(x => x.Pago)
             .HasColumnName("PAGO")
             .HasColumnType("bit")
             .IsRequired();

            comprovante.Property(x => x.Valor)
             .HasColumnName("VALOR")
             .HasColumnType("decimal(18,2)")
             .IsRequired();

            comprovante.Property(x => x.ValorPago)
             .HasColumnName("VALOR_PAGO")
             .HasColumnType("decimal(18,2)");

            comprovante.Property(x => x.NumeroAutenticacao)
             .HasColumnName("NUM_AUTENTICACAO")
             .HasMaxLength(100)
             .IsRequired();

            comprovante.Property(x => x.Protocolo)
             .HasColumnName("PROTOCOLO")
             .HasMaxLength(100);

            comprovante.Property(x => x.IdentificadorTransacao)
             .HasColumnName("ID_TRANSACAO")
             .HasMaxLength(150);

            comprovante.Property(x => x.Emissor)
             .HasColumnName("EMISSOR")
             .HasMaxLength(100);

            comprovante.Property(x => x.Hash)
             .HasColumnName("HASH")
             .HasMaxLength(64);

            comprovante.Property(x => x.Observacoes)
             .HasColumnName("OBSERVACOES")
             .HasMaxLength(1000);

            comprovante.Property(x => x.Arquivo)
             .HasColumnName("ARQUIVO")
             .HasColumnType("varbinary(max)");

            comprovante.Property(x => x.MimeType)
             .HasColumnName("MIME_TYPE")
             .HasMaxLength(100);

            comprovante.Property(x => x.DataPagamento)
             .HasColumnName("DATA_PAGAMENTO")
             .HasColumnType("datetimeoffset(6)")
             .IsRequired();

            comprovante.Property(x => x.DataGeracao)
             .HasColumnName("DATA_GERACAO")
             .HasColumnType("datetimeoffset(6)")
             .IsRequired();

            comprovante.HasOne(x => x.Cobranca)
             .WithOne()
             .HasForeignKey<Comprovante>(x => x.CobrancaId)
             .HasConstraintName("FK_COMPROVANTE_COBRANCA")
             .OnDelete(DeleteBehavior.Restrict);

            comprovante.HasOne(x => x.Cliente)
             .WithMany()
             .HasForeignKey(x => x.ClienteId)
             .HasConstraintName("FK_COMPROVANTE_CLIENTE")
             .OnDelete(DeleteBehavior.Restrict);

            comprovante.HasIndex(x => x.CobrancaId)
             .IsUnique()
             .HasDatabaseName("UX_COMPROVANTE_COBRANCA");

            comprovante.HasIndex(x => new { x.ClienteId, x.DataPagamento })
             .HasDatabaseName("IX_COMP_CLIENTE_PAGTO");

            comprovante.HasIndex(x => x.NumeroAutenticacao)
             .HasDatabaseName("IX_COMP_AUTENTICACAO");

            comprovante.HasIndex(x => x.IdentificadorTransacao)
             .HasDatabaseName("IX_COMP_IDTRANS");
        }
    }
}
