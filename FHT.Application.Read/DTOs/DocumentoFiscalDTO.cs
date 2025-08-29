using System;

namespace FHT.Application.Read.DTOs
{
    public class DocumentoFiscalDTO
    {
        public long DocumentoFiscalId { get; set; }
        public long ClienteId { get; set; }
        public TipoDocumentoFiscalDTO Tipo { get; set; }
        public string Numero { get; set; }
        public string? OrgaoEmissor { get; set; }
        public string? UfEmissor { get; set; }
        public DateTimeOffset? DataEmissao { get; set; }
        public DateTimeOffset? Validade { get; set; }
        public bool Principal { get; set; }
        public DateTimeOffset DataCadastro { get; set; }
        public DateTimeOffset DataAtualizacao { get; set; }
    }
}
