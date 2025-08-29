using System;

namespace FHT.Domain.Entities
{
    public sealed class Compliance
    {
        public long ComplianceId { get; set; }
        public long ClienteId { get; set; }

        public StatusKyc StatusKyc { get; set; } = StatusKyc.Pendente;
        public NivelRisco NivelRisco { get; set; } = NivelRisco.Baixo;

        public bool PessoaPoliticamenteExposto { get; set; }
        public bool PossuiRestricaoSancoes { get; set; }
        public string FonteAnalise { get; set; }
        public string Observacao { get; set; }

        public DateTimeOffset? DataAnalise { get; set; }
        public DateTimeOffset? DataExpiracao { get; set; }

        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DataAtualizacao { get; set; } = DateTimeOffset.Now;

        public Cliente Cliente { get; set; }
    }
}
