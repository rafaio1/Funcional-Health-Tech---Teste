using System;

namespace FHT.Domain.Entities
{
    public enum AcaoAuditoria
    {
        INSERT, UPDATE, DELETE, LOGIN, LOGOUT, STATUS, OTHER, MIGRACAO
    }

    public sealed class Auditoria
    {
        public long AuditoriaId { get; set; }
        public string Entidade { get; set; } = default!;
        public string EntidadeId { get; set; } = default!;
        public AcaoAuditoria Acao { get; set; }
        public string Motivo { get; set; }

        public long? UsuarioId { get; set; }
        public string UsuarioLogin { get; set; }
        public string CorrelacaoId { get; set; }
        public string SessionId { get; set; }
        public string OrigemIp { get; set; }
        public string UserAgent { get; set; }

        public string DadosAntes { get; set; }
        public string DadosDepois { get; set; }

        public bool Sucesso { get; set; } = true;
        public string ErroMsg { get; set; }

        public DateTimeOffset DataEvento { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DataInsercao { get; set; } = DateTimeOffset.Now;
    }
}
