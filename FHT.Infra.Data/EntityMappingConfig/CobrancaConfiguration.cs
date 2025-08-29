using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class CobrancaConfiguration : IEntityTypeConfiguration<Cobranca>
    {
        public void Configure(EntityTypeBuilder<Cobranca> cobranca)
        {
            cobranca.ToTable("COBRANCA");

            cobranca.HasKey(x => x.CobrancaId)
             .HasName("PK_COBRANCA");

            cobranca.Property(x => x.CobrancaId)
             .HasColumnName("COBRANCA_ID")
             .UseIdentityColumn();

            cobranca.Property(x => x.ClienteId)
             .HasColumnName("CLIENTE_ID")
             .IsRequired();

            cobranca.Property(x => x.Metodo)
             .HasColumnName("METODO")
             .HasConversion<string>()
             .HasMaxLength(20)
             .IsRequired();

            cobranca.Property(x => x.Situacao)
             .HasColumnName("SITUACAO")
             .HasConversion<string>()
             .HasMaxLength(20)
             .IsRequired();

            cobranca.Property(x => x.Pago)
             .HasColumnName("PAGO")
             .HasColumnType("bit")
             .IsRequired();

            cobranca.Property(x => x.Valor)
             .HasColumnName("VALOR")
             .HasColumnType("decimal(18,2)")
             .IsRequired();

            cobranca.Property(x => x.Desconto)
             .HasColumnName("DESCONTO")
             .HasColumnType("decimal(18,2)");

            cobranca.Property(x => x.Multa)
             .HasColumnName("MULTA")
             .HasColumnType("decimal(18,2)");

            cobranca.Property(x => x.Juros)
             .HasColumnName("JUROS")
             .HasColumnType("decimal(18,2)");

            cobranca.Property(x => x.ValorPago)
             .HasColumnName("VALOR_PAGO")
             .HasColumnType("decimal(18,2)");

            cobranca.Property(x => x.Descricao)
             .HasColumnName("DESCRICAO")
             .HasMaxLength(255);

            cobranca.Property(x => x.ReferenciaExterna)
             .HasColumnName("REFERENCIA_EXTERNA")
             .HasMaxLength(100);

            cobranca.Property(x => x.CodigoBarras)
             .HasColumnName("CODIGO_BARRAS")
             .HasMaxLength(120);

            cobranca.Property(x => x.LinhaDigitavel)
             .HasColumnName("LINHA_DIGITAVEL")
             .HasMaxLength(256);

            cobranca.Property(x => x.NossoNumero)
             .HasColumnName("NOSSO_NUMERO")
             .HasMaxLength(50);

            cobranca.Property(x => x.PixTxId)
             .HasColumnName("PIX_TXID")
             .HasMaxLength(100);

            cobranca.Property(x => x.PixChave)
             .HasColumnName("PIX_CHAVE")
             .HasMaxLength(120);

            cobranca.Property(x => x.IdentificadorTransacao)
             .HasColumnName("ID_TRANSACAO")
             .HasMaxLength(150);

            cobranca.Property(x => x.Gateway)
             .HasColumnName("GATEWAY")
             .HasMaxLength(100);

            cobranca.Property(x => x.Metadados)
             .HasColumnName("METADADOS")
             .HasMaxLength(4000);

            cobranca.Property(x => x.DataEmissao)
             .HasColumnName("DATA_EMISSAO")
             .HasColumnType("datetimeoffset(6)")
             .IsRequired();

            cobranca.Property(x => x.DataVencimento)
             .HasColumnName("DATA_VENCIMENTO")
             .HasColumnType("datetimeoffset(6)");

            cobranca.Property(x => x.DataPagamento)
             .HasColumnName("DATA_PAGAMENTO")
             .HasColumnType("datetimeoffset(6)");

            cobranca.Property(x => x.DataCadastro)
             .HasColumnName("DATA_CADASTRO")
             .HasColumnType("datetimeoffset(6)")
             .IsRequired();

            cobranca.Property(x => x.DataAtualizacao)
             .HasColumnName("DATA_ATUALIZACAO")
             .HasColumnType("datetimeoffset(6)")
             .IsRequired();

            cobranca.HasOne(x => x.Cliente)
             .WithMany()
             .HasForeignKey(x => x.ClienteId)
             .HasConstraintName("FK_COBRANCA_CLIENTE")
             .OnDelete(DeleteBehavior.Restrict);

            cobranca.HasIndex(x => new { x.ClienteId, x.Metodo })
             .HasDatabaseName("IX_COB_CLIENTE_METODO");

            cobranca.HasIndex(x => new { x.Situacao, x.Pago })
             .HasDatabaseName("IX_COB_SITUACAO_PAGO");

            cobranca.HasIndex(x => x.DataVencimento)
             .HasDatabaseName("IX_COB_VENC");

            cobranca.HasIndex(x => x.ReferenciaExterna)
             .HasDatabaseName("IX_COB_REF_EXT");
        }
    }
}
