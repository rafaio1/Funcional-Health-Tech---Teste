using System;

namespace FHT.Application.Read.DTOs
{
    public class DadoPessoalDTO
    {
        public long DadoPessoalId { get; set; }
        public long ClienteId { get; set; }
        public DateTimeOffset? DataNascimento { get; set; }
        public string? NomeSocial { get; set; }
        public string? NomeMae { get; set; }
        public string? NomePai { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Genero { get; set; }
        public string? Nacionalidade { get; set; }
        public string? Profissao { get; set; }
        public decimal? RendaMensal { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset DataAtualizacao { get; set; }
    }
}
