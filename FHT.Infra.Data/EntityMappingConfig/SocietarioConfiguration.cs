using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class SocietarioConfiguration : IEntityTypeConfiguration<Societario>
    {
        public void Configure(EntityTypeBuilder<Societario> societario)
        {
            societario.ToTable("SOCIETARIO");

            societario.HasKey(x => x.SocietarioId)
                      .HasName("PK_SOCIETARIO");

            societario.Property(x => x.SocietarioId)
                      .HasColumnName("SOCIETARIO_ID")
                      .UseIdentityColumn(); 

            societario.Property(x => x.ClienteId)
                      .HasColumnName("CLIENTE_ID")
                      .IsRequired();

            societario.Property(x => x.Nome)
                      .HasColumnName("NOME")
                      .HasMaxLength(200)
                      .IsRequired();

            societario.Property(x => x.Documento)
                      .HasColumnName("DOCUMENTO")
                      .HasMaxLength(20)
                      .IsRequired();

            societario.Property(x => x.CargoFuncao)
                      .HasColumnName("CARGO_FUNCAO")
                      .HasMaxLength(100);

            societario.Property(x => x.ParticipacaoPercentual)
                      .HasColumnName("PARTICIPACAO_PERCENTUAL")
                      .HasPrecision(5, 2);

            societario.Property(x => x.RepresentanteLegal)
                      .HasColumnName("REPRESENTANTE_LEGAL")
                      .HasColumnType("bit")
                      .IsRequired();

            societario.Property(x => x.DataEntrada)
                      .HasColumnName("DATA_ENTRADA")
                      .HasColumnType("datetimeoffset(6)");

            societario.Property(x => x.DataSaida)
                      .HasColumnName("DATA_SAIDA")
                      .HasColumnType("datetimeoffset(6)");

            societario.Property(x => x.DataCadastro)
                      .HasColumnName("DATA_CADASTRO")
                      .HasColumnType("datetimeoffset(6)")
                      .IsRequired();

            societario.Property(x => x.DataAtualizacao)
                      .HasColumnName("DATA_ATUALIZACAO")
                      .HasColumnType("datetimeoffset(6)")
                      .IsRequired();

            societario.HasIndex(x => x.ClienteId)
                      .HasDatabaseName("IX_SOCIETARIO_CLIENTE");
        }
    }
}
