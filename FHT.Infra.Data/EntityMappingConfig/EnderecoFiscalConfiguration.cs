using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class EnderecoFiscalConfiguration : IEntityTypeConfiguration<EnderecoFiscal>
    {
        public void Configure(EntityTypeBuilder<EnderecoFiscal> endereco)
        {
            endereco.ToTable("ENDERECO_FISCAL");

            endereco.HasKey(x => x.EnderecoFiscalId)
                    .HasName("PK_ENDERECO_FISCAL");

            endereco.Property(x => x.EnderecoFiscalId)
                    .HasColumnName("ENDERECO_FISCAL_ID")
                    .UseIdentityColumn(); 

            endereco.Property(x => x.ClienteId)
                    .HasColumnName("CLIENTE_ID")
                    .IsRequired();

            endereco.Property(x => x.Tipo)
                    .HasColumnName("TIPO_ENDERECO")
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

            endereco.Property(x => x.Logradouro)
                    .HasColumnName("LOGRADOURO")
                    .HasMaxLength(200)
                    .IsRequired();

            endereco.Property(x => x.Numero)
                    .HasColumnName("NUMERO")
                    .HasMaxLength(20);

            endereco.Property(x => x.Complemento)
                    .HasColumnName("COMPLEMENTO")
                    .HasMaxLength(100);

            endereco.Property(x => x.Bairro)
                    .HasColumnName("BAIRRO")
                    .HasMaxLength(100)
                    .IsRequired();

            endereco.Property(x => x.Municipio)
                    .HasColumnName("MUNICIPIO")
                    .HasMaxLength(100)
                    .IsRequired();

            endereco.Property(x => x.Uf)
                    .HasColumnName("UF")
                    .HasMaxLength(2)
                    .IsRequired();

            endereco.Property(x => x.Cep)
                    .HasColumnName("CEP")
                    .HasMaxLength(10)  
                    .IsRequired();

            endereco.Property(x => x.Pais)
                    .HasColumnName("PAIS")
                    .HasMaxLength(60);

            endereco.Property(x => x.CodigoIbgeMunicipio)
                    .HasColumnName("CODIGO_IBGE_MUNICIPIO")
                    .HasMaxLength(7);  

            endereco.Property(x => x.Principal)
                    .HasColumnName("PRINCIPAL")
                    .HasColumnType("bit")
                    .IsRequired();

            endereco.Property(x => x.DataCadastro)
                    .HasColumnName("DATA_CADASTRO")
                    .HasColumnType("datetimeoffset(6)")
                    .IsRequired();

            endereco.Property(x => x.DataAtualizacao)
                    .HasColumnName("DATA_ATUALIZACAO")
                    .HasColumnType("datetimeoffset(6)")
                    .IsRequired();

            endereco.HasIndex(x => new { x.ClienteId, x.Tipo, x.Principal })
                    .HasDatabaseName("IX_END_FISCAL_CLIENTE_TIPO_PRINC");
        }
    }
}
