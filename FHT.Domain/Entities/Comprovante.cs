using System;

namespace FHT.Domain.Entities
{
    public sealed class Comprovante
    {
        public long ComprovanteId { get; set; }
        public long CobrancaId { get; set; }
        public long ClienteId { get; set; }

        public MetodoCobranca Metodo { get; set; }
        public SituacaoCobranca SituacaoNoMomento { get; set; } = SituacaoCobranca.Pago;

        public bool Pago { get; set; } = true;

        public decimal Valor { get; set; }
        public decimal? ValorPago { get; set; }

        public string NumeroAutenticacao { get; set; }
        public string Protocolo { get; set; }
        public string IdentificadorTransacao { get; set; }
        public string Emissor { get; set; }
        public string Hash { get; set; }
        public string Observacoes { get; set; }

        public byte[] Arquivo { get; set; }
        public string MimeType { get; set; }

        public DateTimeOffset DataPagamento { get; set; }
        public DateTimeOffset DataGeracao { get; set; } = DateTimeOffset.Now;

        public Cobranca Cobranca { get; set; }
        public Cliente Cliente { get; set; }
    }
}