using System;

namespace FHT.Application.Read.DTOs
{
    public class ComplianceDTO
    {
        public long ComplianceId { get; set; }
        public long ClienteId { get; set; }
        public StatusKycDTO StatusKyc { get; set; }
        public NivelRiscoDTO NivelRisco { get; set; }
        public bool Pep { get; set; }
        public bool PossuiRestricaoSancoes { get; set; }
        public string? FonteAnalise { get; set; }
        public string? Observacao { get; set; }
        public DateTimeOffset? DataAnalise { get; set; }
        public DateTimeOffset? DataExpiracao { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset DataAtualizacao { get; set; }
    }
}
