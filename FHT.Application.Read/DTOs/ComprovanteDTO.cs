using System;

namespace FHT.Application.Read.DTOs
{
    public class ComprovanteDTO
    {
        public long ComprovanteId { get; set; }
        public long CobrancaId { get; set; }
        public long ClienteId { get; set; }
        public string NumeroAutenticacao { get; set; }
        public string? IdentificadorTransacao { get; set; }
        public decimal Valor { get; set; }
        public decimal? ValorPago { get; set; }
        public DateTimeOffset DataPagamento { get; set; }
        public DateTimeOffset DataGeracao { get; set; }
        public string? Emissor { get; set; }
    }
}
