using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class DocumentoFiscalConfiguration : IEntityTypeConfiguration<DocumentoFiscal>
    {
        public void Configure(EntityTypeBuilder<DocumentoFiscal> documentoFiscal)
        {
            documentoFiscal.ToTable("DOCUMENTO_FISCAL");

            documentoFiscal.HasKey(x => x.DocumentoFiscalId)
                           .HasName("PK_DOCUMENTO_FISCAL");

            documentoFiscal.Property(x => x.DocumentoFiscalId)
                           .HasColumnName("DOCUMENTO_FISCAL_ID")
                           .UseIdentityColumn(); 

            documentoFiscal.Property(x => x.ClienteId)
                           .HasColumnName("CLIENTE_ID")
                           .IsRequired();

            documentoFiscal.Property(x => x.Tipo)
                           .HasColumnName("TIPO_DOCUMENTO")
                           .HasConversion<string>()
                           .HasMaxLength(20)
                           .IsRequired();

            documentoFiscal.Property(x => x.Numero)
                           .HasColumnName("NUMERO")
                           .HasMaxLength(40)
                           .IsRequired();

            documentoFiscal.Property(x => x.OrgaoEmissor)
                           .HasColumnName("ORGAO_EMISSOR")
                           .HasMaxLength(50);

            documentoFiscal.Property(x => x.UfEmissor)
                           .HasColumnName("UF_EMISSOR")
                           .HasMaxLength(2);

            documentoFiscal.Property(x => x.DataEmissao)
                           .HasColumnName("DATA_EMISSAO")
                           .HasColumnType("datetimeoffset(6)");

            documentoFiscal.Property(x => x.Validade)
                           .HasColumnName("DATA_VALIDADE")
                           .HasColumnType("datetimeoffset(6)");

            documentoFiscal.Property(x => x.Principal)
                           .HasColumnName("PRINCIPAL")
                           .IsRequired();

            documentoFiscal.Property(x => x.DataCadastro)
                           .HasColumnName("DATA_CADASTRO")
                           .HasColumnType("datetimeoffset(6)")
                           .IsRequired();

            documentoFiscal.Property(x => x.DataAtualizacao)
                           .HasColumnName("DATA_ATUALIZACAO")
                           .HasColumnType("datetimeoffset(6)")
                           .IsRequired();

            documentoFiscal.HasIndex(x => new { x.ClienteId, x.Tipo, x.Numero })
                           .HasDatabaseName("UX_DOCFISCAL_CLIENTE_TIPO_NUMERO")
                           .IsUnique();
        }
    }
}
