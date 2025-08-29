using System;

namespace FHT.Application.Read.DTOs
{
    public class EnderecoFiscalDTO
    {
        public long EnderecoFiscalId { get; set; }
        public long ClienteId { get; set; }
        public TipoEnderecoDTO Tipo { get; set; }
        public string Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public string Cep { get; set; }
        public string? Pais { get; set; }
        public string? CodigoIbgeMunicipio { get; set; }
        public bool Principal { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset DataAtualizacao { get; set; }
    }
}
