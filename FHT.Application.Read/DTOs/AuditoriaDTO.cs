using System;

namespace FHT.Application.Read.DTOs
{
    public class AuditoriaDTO
    {
        public long AuditoriaId { get; set; }
        public string Entidade { get; set; }
        public string EntidadeId { get; set; }
        public AcaoAuditoriaDTO Acao { get; set; }
        public string? Motivo { get; set; }
        public long? UsuarioId { get; set; }
        public string? UsuarioLogin { get; set; }
        public string? CorrelacaoId { get; set; }
        public string? SessionId { get; set; }
        public string? OrigemIp { get; set; }
        public string? UserAgent { get; set; }
        public string? DadosAntes { get; set; }
        public string? DadosDepois { get; set; }
        public bool Sucesso { get; set; }
        public string? ErroMsg { get; set; }
        public DateTimeOffset DtEvento { get; set; }
        public DateTimeOffset DtInsercao { get; set; }
    }
}
