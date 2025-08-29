using System;

namespace FHT.Domain.Entities
{
    public sealed class DocumentoFiscal
    {
        public long DocumentoFiscalId { get; set; }
        public long ClienteId { get; set; }

        public TipoDocumentoFiscal Tipo { get; set; }
        public string Numero { get; set; }
        public string OrgaoEmissor { get; set; }
        public string UfEmissor { get; set; }
        public DateTimeOffset? DataEmissao { get; set; }
        public DateTimeOffset? Validade { get; set; }
        public bool Principal { get; set; } = false;

        public DateTimeOffset DataCadastro { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset DataAtualizacao { get; set; } = DateTimeOffset.Now;

        public Cliente Cliente { get; set; }
    }
}
