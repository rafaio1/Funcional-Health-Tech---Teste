using System;

namespace FHT.Domain.Entities
{
    public sealed class EnderecoFiscal
    {
        public long EnderecoFiscalId { get; set; }
        public long ClienteId { get; set; }

        public TipoEndereco Tipo { get; set; } = TipoEndereco.Residencial;

        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public string Pais { get; set; }
        public string CodigoIbgeMunicipio { get; set; }

        public bool Principal { get; set; } = false;

        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DataAtualizacao { get; set; } = DateTimeOffset.Now;

        public Cliente Cliente { get; set; }
    }
}
