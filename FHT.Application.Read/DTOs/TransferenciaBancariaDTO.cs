using FHT.Domain.Entities;
using System;

namespace FHT.Application.Read.DTOs
{
    public class TransferenciaBancariaDTO
    {
        public long TransferenciaId { get; set; }
        public long ClienteId { get; set; }
        public long ContaId { get; set; }
        public TipoTransferenciaDTO Tipo { get; set; }
        public StatusTransferenciaDTO Status { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public string IdentificadorTransacao { get; set; }
        public string PixChave { get; set; }
        public string BancoDestino { get; set; }
        public string AgenciaDestino { get; set; }
        public string ContaDestino { get; set; }
        public string DocumentoTitularDestino { get; set; }
        public string NomeTitularDestino { get; set; }
        public string CodigoBarras { get; set; }
        public string LinhaDigitavel { get; set; }

        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string MensagemErro { get; set; }

        public ClienteDTO Cliente { get; set; }
        public ContaDTO Conta { get; set; }
    }
}
