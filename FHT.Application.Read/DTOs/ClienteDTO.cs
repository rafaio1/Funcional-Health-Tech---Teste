using System;

namespace FHT.Application.Read.DTOs
{
    public class ClienteDTO
    {
        public long ClienteId { get; set; }
        public TipoClienteDTO Tipo { get; set; }
        public StatusClienteDTO Status { get; set; }
        public string Nome { get; set; }
        public decimal? Saldo { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset DataAtualizacao { get; set; }
    }
}
