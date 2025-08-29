using System;
namespace FHT.Application.Read.DTOs
{
    public class CobrancaDTO
    {
        public long CobrancaId { get; set; }
        public long ClienteId { get; set; }
        public MetodoCobrancaDTO Metodo { get; set; }
        public SituacaoCobrancaDTO Situacao { get; set; }
        public bool Pago { get; set; }
        public decimal Valor { get; set; }
        public decimal? ValorPago { get; set; }
        public DateTimeOffset DataEmissao { get; set; }
        public DateTimeOffset? DataVencimento { get; set; }
        public DateTimeOffset? DataPagamento { get; set; }
        public string? ReferenciaExterna { get; set; }
    }
}