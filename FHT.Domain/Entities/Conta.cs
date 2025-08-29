using System;

namespace FHT.Domain.Entities
{
    public sealed class Conta
    {
        public long ContaId { get; set; }
        public long ClienteId { get; set; }

        public TipoConta Tipo { get; set; }
        public StatusConta Status { get; set; } = StatusConta.Ativa;

        public string Agencia { get; set; }
        public string Numero { get; set; }
        public string Digito { get; set; }

        public decimal Saldo { get; set; } = 0m;

        public DateTimeOffset DataAbertura { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? DataEncerramento { get; set; }

        public Cliente Cliente { get; set; }
    }
}
