using FHT.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHT.Infra.Data.EntityMappingConfig
{
    public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
    {
        public void Configure(EntityTypeBuilder<Auditoria> auditoria)
        {
            auditoria.ToTable("AUDITORIA");

            auditoria.HasKey(x => x.AuditoriaId)
                     .HasName("PK_AUDITORIA");

            auditoria.Property(x => x.AuditoriaId)
                     .HasColumnName("AUDITORIA_ID")
                     .UseIdentityColumn();                   

            auditoria.Property(x => x.Entidade)
                     .HasColumnName("ENTIDADE")
                     .HasMaxLength(100)
                     .IsRequired();

            auditoria.Property(x => x.EntidadeId)
                     .HasColumnName("ENTIDADE_ID")
                     .HasMaxLength(100)
                     .IsRequired();

            auditoria.Property(x => x.Acao)
                     .HasColumnName("ACAO")
                     .HasConversion<string>()
                     .HasMaxLength(20)
                     .IsRequired();

            auditoria.Property(x => x.Motivo)
                     .HasColumnName("MOTIVO")
                     .HasMaxLength(4000);

            auditoria.Property(x => x.UsuarioId)
                     .HasColumnName("USUARIO_ID");

            auditoria.Property(x => x.UsuarioLogin)
                     .HasColumnName("USUARIO_LOGIN")
                     .HasMaxLength(150);

            auditoria.Property(x => x.CorrelacaoId)
                     .HasColumnName("CORRELACAO_ID")
                     .HasMaxLength(64);

            auditoria.Property(x => x.SessionId)
                     .HasColumnName("SESSION_ID")
                     .HasMaxLength(64);

            auditoria.Property(x => x.OrigemIp)
                     .HasColumnName("ORIGEM_IP")
                     .HasMaxLength(45);

            auditoria.Property(x => x.UserAgent)
                     .HasColumnName("USER_AGENT")
                     .HasMaxLength(400);

            auditoria.Property(x => x.DadosAntes)
                     .HasColumnName("DADOS_ANTES")
                     .HasColumnType("nvarchar(max)");

            auditoria.Property(x => x.DadosDepois)
                     .HasColumnName("DADOS_DEPOIS")
                     .HasColumnType("nvarchar(max)");

            auditoria.Property(x => x.Sucesso)
                     .HasColumnName("SUCESSO")
                     .HasColumnType("bit")
                     .IsRequired();

            auditoria.Property(x => x.ErroMsg)
                     .HasColumnName("ERRO_MSG")
                     .HasMaxLength(1000);

            auditoria.Property(x => x.DataEvento)
                     .HasColumnName("DATA_EVENTO")
                     .HasColumnType("datetimeoffset(6)")
                     .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                     .IsRequired();

            auditoria.Property(x => x.DataInsercao)
                     .HasColumnName("DATA_INSERCAO")
                     .HasColumnType("datetimeoffset(6)")
                     .HasDefaultValueSql("SYSDATETIMEOFFSET()")
                     .IsRequired();

            auditoria.HasIndex(x => new { x.Entidade, x.EntidadeId })
                     .HasDatabaseName("IX_AUD_ENT");

            auditoria.HasIndex(x => x.DataEvento)
                     .HasDatabaseName("IX_AUD_DATA");

            auditoria.HasIndex(x => x.CorrelacaoId)
                     .HasDatabaseName("IX_AUD_COR");

            auditoria.HasIndex(x => x.UsuarioLogin)
                     .HasDatabaseName("IX_AUD_USR");
        }
    }
}
