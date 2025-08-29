using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class ComplianceConfiguration : IEntityTypeConfiguration<Compliance>
    {
        public void Configure(EntityTypeBuilder<Compliance> compliance)
        {
            compliance.ToTable("COMPLIANCE");

            compliance.HasKey(x => x.ComplianceId)
                      .HasName("PK_COMPLIANCE");

            compliance.Property(x => x.ComplianceId)
                      .HasColumnName("COMPLIANCE_ID")
                      .UseIdentityColumn();

            compliance.Property(x => x.ClienteId)
                      .HasColumnName("CLIENTE_ID")
                      .IsRequired();

            compliance.Property(x => x.StatusKyc)
                      .HasColumnName("STATUS_KYC")
                      .HasConversion<string>()
                      .HasMaxLength(12)
                      .IsRequired();

            compliance.Property(x => x.NivelRisco)
                      .HasColumnName("NIVEL_RISCO")
                      .HasConversion<string>()
                      .HasMaxLength(10)
                      .IsRequired();

            compliance.Property(x => x.PessoaPoliticamenteExposto)
                      .HasColumnName("PESSOA_POLITICAMENTE_EXPOSTA")
                      .HasColumnType("bit")
                      .IsRequired();

            compliance.Property(x => x.PossuiRestricaoSancoes)
                      .HasColumnName("RESTRICAO_SANCOES")
                      .HasColumnType("bit")
                      .IsRequired();

            compliance.Property(x => x.FonteAnalise)
                      .HasColumnName("FONTE_ANALISE")
                      .HasMaxLength(100);

            compliance.Property(x => x.Observacao)
                      .HasColumnName("OBSERVACAO")
                      .HasMaxLength(1000);

            compliance.Property(x => x.DataAnalise)
                      .HasColumnName("DATA_ANALISE")
                      .HasColumnType("datetimeoffset(6)");

            compliance.Property(x => x.DataExpiracao)
                      .HasColumnName("DATA_EXPIRACAO")
                      .HasColumnType("datetimeoffset(6)");

            compliance.Property(x => x.DataCadastro)
                      .HasColumnName("DATA_CADASTRO")
                      .HasColumnType("datetimeoffset(6)")
                      .IsRequired();

            compliance.Property(x => x.DataAtualizacao)
                      .HasColumnName("DATA_ATUALIZACAO")
                      .HasColumnType("datetimeoffset(6)")
                      .IsRequired();

             compliance.HasOne(x => x.Cliente)
                       .WithMany(c => c.Compliance)
                       .HasForeignKey(x => x.ClienteId)
                       .HasConstraintName("FK_COMPLIANCE_CLIENTE")
                       .OnDelete(DeleteBehavior.Cascade);

            compliance.HasIndex(x => new { x.ClienteId, x.StatusKyc, x.NivelRisco })
                      .HasDatabaseName("IX_COMPLIANCE_CLIENTE_STATUS_RISCO");
        }
    }
}
