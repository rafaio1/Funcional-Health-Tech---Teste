using System;
using System.Collections.Generic;

namespace FHT.Domain.Entities
{
    public sealed class Cliente
    {
        public long ClienteId { get; set; }
        public TipoCliente Tipo { get; set; }
        public StatusCliente Status { get; set; } = StatusCliente.Ativo;

        public string Nome { get; set; }

        // Isolado para não violar a LGPD em alguns casos, acessado somente quando for realmente necessário
        public DadoPessoal DadosPessoais { get; set; }

        public ICollection<Compliance> Compliance { get; set; }
        public ICollection<Contato> Contato { get; set; }
        public ICollection<DocumentoFiscal> DocumentosFiscais { get; set; }
        public ICollection<EnderecoFiscal> Endereco { get; set; }
        public ICollection<Societario> Societario { get; set; }

        public decimal? Saldo { get; set; }

        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DataAtualizacao { get; set; } = DateTimeOffset.Now;

        public ICollection<Conta> Contas { get; set; }
    }
}
