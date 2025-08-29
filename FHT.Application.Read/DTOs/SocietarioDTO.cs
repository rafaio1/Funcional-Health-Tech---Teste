using System;

namespace FHT.Application.Read.DTOs
{
    public class SocietarioDTO
    {
        public long SocietarioId { get; set; }
        public long ClienteId { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string? CargoFuncao { get; set; }
        public decimal? ParticipacaoPercentual { get; set; }
        public bool RepresentanteLegal { get; set; }
        public DateTimeOffset? DataEntrada { get; set; }
        public DateTimeOffset? DataSaida { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset DataAtualizacao { get; set; }
    }
}
