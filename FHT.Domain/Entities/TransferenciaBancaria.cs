using System;

namespace FHT.Domain.Entities
{
    public class TransferenciaBancaria
    {
        public long TransferenciaId { get; set; }
        public long ClienteId { get; set; }
        public long ContaId { get; set; }

        public TipoTransferencia Tipo { get; set; }
        public StatusTransferencia Status { get; set; }

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

        public Cliente Cliente { get; set; }
        public Conta Conta { get; set; }
    }
}
