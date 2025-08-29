using System;

namespace FHT.Domain.Entities
{
    public sealed class Societario
    {
        public long SocietarioId { get; set; }
        public long ClienteId { get; set; }

        public string Nome { get; set; }
        public string Documento { get; set; }
        public string CargoFuncao { get; set; }
        public decimal? ParticipacaoPercentual { get; set; }
        public bool RepresentanteLegal { get; set; } = false;

        public DateTimeOffset? DataEntrada { get; set; }
        public DateTimeOffset? DataSaida { get; set; }

        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DataAtualizacao { get; set; } = DateTimeOffset.Now;

        public Cliente Cliente { get; set; }
    }
}
