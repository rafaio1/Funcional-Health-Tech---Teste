using System;

namespace FHT.Domain.Entities
{
    public sealed class Contato
    {
        public long ContatoId { get; set; }
        public long ClienteId { get; set; }

        public TipoContato Tipo { get; set; }
        public string Valor { get; set; }
        public bool Principal { get; set; } = false;
        public string Observacao { get; set; }

        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DataAtualizacao { get; set; } = DateTimeOffset.Now;

        public Cliente Cliente { get; set; }
    }
}
