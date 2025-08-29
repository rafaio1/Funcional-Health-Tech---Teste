using System;

namespace FHT.Domain.Entities
{
    public sealed class Cobranca
    {
        public long CobrancaId { get; set; }
        public long ClienteId { get; set; }

        public MetodoCobranca Metodo { get; set; }
        public SituacaoCobranca Situacao { get; set; } = SituacaoCobranca.Pendente;

        public bool Pago { get; set; } = false;

        public decimal Valor { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? Multa { get; set; }
        public decimal? Juros { get; set; }
        public decimal? ValorPago { get; set; }

        public string Descricao { get; set; }
        public string ReferenciaExterna { get; set; }

        public string CodigoBarras { get; set; }
        public string LinhaDigitavel { get; set; }
        public string NossoNumero { get; set; }
        public string PixTxId { get; set; }
        public string PixChave { get; set; }
        public string IdentificadorTransacao { get; set; }
        public string Gateway { get; set; }

        public string Metadados { get; set; }

        public DateTimeOffset DataEmissao { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? DataVencimento { get; set; }
        public DateTimeOffset? DataPagamento { get; set; }
        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DataAtualizacao { get; set; } = DateTimeOffset.Now;

        public Cliente Cliente { get; set; }
    }
}
