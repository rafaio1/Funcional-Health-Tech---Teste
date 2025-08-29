using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class DadoPessoalConfiguration : IEntityTypeConfiguration<DadoPessoal>
    {
        public void Configure(EntityTypeBuilder<DadoPessoal> dadoPessoal)
        {
            dadoPessoal.ToTable("DADO_PESSOAL");

            dadoPessoal.HasKey(x => x.DadoPessoalId)
                       .HasName("PK_DADO_PESSOAL");

            dadoPessoal.Property(x => x.DadoPessoalId)
                       .HasColumnName("DADO_PESSOAL_ID")
                       .UseIdentityColumn(); 

            dadoPessoal.Property(x => x.ClienteId)
                       .HasColumnName("CLIENTE_ID")
                       .IsRequired();

            dadoPessoal.Property(x => x.DataNascimento)
                       .HasColumnName("DATA_NASCIMENTO")
                       .HasColumnType("datetimeoffset(6)");

            dadoPessoal.Property(x => x.NomeSocial)
                       .HasColumnName("NOME_SOCIAL")
                       .HasMaxLength(200);

            dadoPessoal.Property(x => x.NomeMae)
                       .HasColumnName("NOME_MAE")
                       .HasMaxLength(200);

            dadoPessoal.Property(x => x.NomePai)
                       .HasColumnName("NOME_PAI")
                       .HasMaxLength(200);

            dadoPessoal.Property(x => x.EstadoCivil)
                       .HasColumnName("ESTADO_CIVIL")
                       .HasMaxLength(30);

            dadoPessoal.Property(x => x.Genero)
                       .HasColumnName("GENERO")
                       .HasMaxLength(30);

            dadoPessoal.Property(x => x.Nacionalidade)
                       .HasColumnName("NACIONALIDADE")
                       .HasMaxLength(60);

            dadoPessoal.Property(x => x.Profissao)
                       .HasColumnName("PROFISSAO")
                       .HasMaxLength(100);

            dadoPessoal.Property(x => x.RendaMensal)
                       .HasColumnName("RENDA_MENSAL")
                       .HasPrecision(18, 2);

            dadoPessoal.Property(x => x.DataCadastro)
                       .HasColumnName("DATA_CADASTRO")
                       .HasColumnType("datetimeoffset(6)")
                       .IsRequired();

            dadoPessoal.Property(x => x.DataAtualizacao)
                       .HasColumnName("DATA_ATUALIZACAO")
                       .HasColumnType("datetimeoffset(6)")
                       .IsRequired();

            dadoPessoal.HasIndex(x => x.ClienteId)
                       .HasDatabaseName("IX_DADO_PESSOAL_CLIENTE");

            dadoPessoal.HasIndex(x => x.ClienteId)
                       .IsUnique()
                       .HasDatabaseName("UX_DADO_PESSOAL_CLIENTE");
        }
    }
}
