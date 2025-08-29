using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class TransferenciaConfiguration : IEntityTypeConfiguration<TransferenciaBancaria>
    {
        public void Configure(EntityTypeBuilder<TransferenciaBancaria> transferencia)
        {
            transferencia.ToTable("TRANSFERENCIA");
            transferencia.HasKey(x => x.TransferenciaId).HasName("PK_TRANSFERENCIA");

            transferencia.Property(x => x.TransferenciaId)
             .HasColumnName("TRANSFERENCIA_ID")
             .ValueGeneratedOnAdd();

            transferencia.Property(x => x.ClienteId).HasColumnName("CLIENTE_ID").IsRequired();
            transferencia.Property(x => x.ContaId).HasColumnName("CONTA_ID").IsRequired();

            transferencia.Property(x => x.Tipo)
             .HasColumnName("TIPO")
             .HasConversion<string>()
             .HasMaxLength(12)
             .IsRequired();

            transferencia.Property(x => x.Status)
             .HasColumnName("STATUS")
             .HasConversion<string>()
             .HasMaxLength(12)
             .IsRequired();

            transferencia.Property(x => x.Valor)
             .HasColumnName("VALOR")
             .HasPrecision(18, 2)
             .IsRequired();

            transferencia.Property(x => x.Descricao).HasColumnName("DESCRICAO").HasMaxLength(500);
            transferencia.Property(x => x.IdentificadorTransacao).HasColumnName("ID_TRANSACAO").HasMaxLength(150);

            transferencia.Property(x => x.PixChave).HasColumnName("PIX_CHAVE").HasMaxLength(140);

            transferencia.Property(x => x.BancoDestino).HasColumnName("BANCO_DESTINO").HasMaxLength(20);
            transferencia.Property(x => x.AgenciaDestino).HasColumnName("AGENCIA_DESTINO").HasMaxLength(20);
            transferencia.Property(x => x.ContaDestino).HasColumnName("CONTA_DESTINO").HasMaxLength(30);
            transferencia.Property(x => x.DocumentoTitularDestino).HasColumnName("DOC_TITULAR_DEST").HasMaxLength(20);
            transferencia.Property(x => x.NomeTitularDestino).HasColumnName("NOME_TITULAR_DEST").HasMaxLength(200);

            transferencia.Property(x => x.CodigoBarras).HasColumnName("CODIGO_BARRAS").HasMaxLength(120);
            transferencia.Property(x => x.LinhaDigitavel).HasColumnName("LINHA_DIGITAVEL").HasMaxLength(256);

            transferencia.Property(x => x.DataSolicitacao).HasColumnName("DATA_SOL").HasColumnType("datetime2").IsRequired();
            transferencia.Property(x => x.DataConclusao).HasColumnName("DATA_CONC").HasColumnType("datetime2");
            transferencia.Property(x => x.MensagemErro).HasColumnName("MSG_ERRO").HasMaxLength(1000);

            transferencia.HasOne(x => x.Cliente)
             .WithMany()
             .HasForeignKey(x => x.ClienteId)
             .HasConstraintName("FK_TRANSFERENCIA_CLIENTE")
             .OnDelete(DeleteBehavior.Restrict);

            transferencia.HasOne(x => x.Conta)
             .WithMany()
             .HasForeignKey(x => x.ContaId)
             .HasConstraintName("FK_TRANSFERENCIA_CONTA")
             .OnDelete(DeleteBehavior.Restrict);

            transferencia.HasIndex(x => new { x.ClienteId, x.ContaId, x.DataSolicitacao })
             .HasDatabaseName("IX_TRANSF_CLIENTE_CONTA_DATA");
        }
    }
}
