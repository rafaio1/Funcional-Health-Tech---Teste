using System;

namespace FHT.Application.Read.DTOs
{
    public class ContatoDTO
    {
        public long ContatoId { get; set; }
        public long ClienteId { get; set; }
        public TipoContatoDTO Tipo { get; set; }
        public string Valor { get; set; }
        public bool Principal { get; set; }
        public string? Observacao { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset DataAtualizacao { get; set; }
    }
}
