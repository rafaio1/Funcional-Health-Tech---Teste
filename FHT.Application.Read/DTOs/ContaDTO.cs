using System;

namespace FHT.Application.Read.DTOs
{
    public class ContaDTO
    {
        public long ContaId { get; set; }
        public long ClienteId { get; set; }
        public TipoContaDTO Tipo { get; set; }
        public StatusContaDTO Status { get; set; }
        public string Agencia { get; set; }
        public string Numero { get; set; }
        public string? Digito { get; set; }
        public decimal Saldo { get; set; }
        public DateTimeOffset DataAbertura { get; set; }
        public DateTimeOffset? DataEncerramento { get; set; }
    }
}
