using AutoMapper;
using FHT.Application.Read.DTOs;
using FHT.Domain.Entities;

namespace FHT.Application.Mapping
{
    public class DtoToDomainProfile : Profile
    {
        public DtoToDomainProfile()
        {
            CreateMap<TipoClienteDTO, TipoCliente>().ConvertUsing(src => (TipoCliente)src);
            CreateMap<StatusClienteDTO, StatusCliente>().ConvertUsing(src => (StatusCliente)src);

            CreateMap<TipoContaDTO, TipoConta>().ConvertUsing(src => (TipoConta)src);
            CreateMap<StatusContaDTO, StatusConta>().ConvertUsing(src => (StatusConta)src);

            CreateMap<StatusKycDTO, StatusKyc>().ConvertUsing(src => (StatusKyc)src);
            CreateMap<NivelRiscoDTO, NivelRisco>().ConvertUsing(src => (NivelRisco)src);

            CreateMap<TipoContatoDTO, TipoContato>().ConvertUsing(src => (TipoContato)src);
            CreateMap<TipoDocumentoFiscalDTO, TipoDocumentoFiscal>().ConvertUsing(src => (TipoDocumentoFiscal)src);
            CreateMap<TipoEnderecoDTO, TipoEndereco>().ConvertUsing(src => (TipoEndereco)src);

            CreateMap<MetodoCobrancaDTO, MetodoCobranca>().ConvertUsing(src => (MetodoCobranca)src);
            CreateMap<SituacaoCobrancaDTO, SituacaoCobranca>().ConvertUsing(src => (SituacaoCobranca)src);

            CreateMap<AcaoAuditoriaDTO, AcaoAuditoria>().ConvertUsing(src => (AcaoAuditoria)src);

            CreateMap<ClienteDTO, Cliente>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoCliente)s.Tipo))
                .ForMember(d => d.Status, m => m.MapFrom(s => (StatusCliente)s.Status))
                .ForMember(d => d.DadosPessoais, m => m.Ignore())
                .ForMember(d => d.Compliance, m => m.Ignore())
                .ForMember(d => d.Contato, m => m.Ignore())
                .ForMember(d => d.DocumentosFiscais, m => m.Ignore())
                .ForMember(d => d.Endereco, m => m.Ignore())
                .ForMember(d => d.Societario, m => m.Ignore())
                .ForMember(d => d.Contas, m => m.Ignore());

            CreateMap<ContaDTO, Conta>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoConta)s.Tipo))
                .ForMember(d => d.Status, m => m.MapFrom(s => (StatusConta)s.Status))
                .ForMember(d => d.Cliente, m => m.Ignore());

            CreateMap<ComplianceDTO, Compliance>()
                .ForMember(d => d.StatusKyc, m => m.MapFrom(s => (StatusKyc)s.StatusKyc))
                .ForMember(d => d.NivelRisco, m => m.MapFrom(s => (NivelRisco)s.NivelRisco))
                .ForMember(d => d.Cliente, m => m.Ignore());

            CreateMap<ContatoDTO, Contato>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoContato)s.Tipo))
                .ForMember(d => d.Cliente, m => m.Ignore());

            CreateMap<DocumentoFiscalDTO, DocumentoFiscal>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoDocumentoFiscal)s.Tipo))
                .ForMember(d => d.Cliente, m => m.Ignore());

            CreateMap<EnderecoFiscalDTO, EnderecoFiscal>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoEndereco)s.Tipo))
                .ForMember(d => d.Cliente, m => m.Ignore());

            CreateMap<SocietarioDTO, Societario>()
                .ForMember(d => d.Cliente, m => m.Ignore());

            CreateMap<DadoPessoalDTO, DadoPessoal>()
                .ForMember(d => d.Cliente, m => m.Ignore());

            CreateMap<AuditoriaDTO, Auditoria>()
                .ForMember(d => d.Acao, m => m.MapFrom(s => (AcaoAuditoria)s.Acao));

            CreateMap<CobrancaDTO, Cobranca>()
                .ForMember(d => d.Metodo, m => m.MapFrom(s => (MetodoCobranca)s.Metodo))
                .ForMember(d => d.Situacao, m => m.MapFrom(s => (SituacaoCobranca)s.Situacao))
                .ForMember(d => d.Cliente, m => m.Ignore());

            CreateMap<ComprovanteDTO, Comprovante>()
                .ForMember(d => d.Metodo, m => m.Ignore())
                .ForMember(d => d.SituacaoNoMomento, m => m.Ignore())
                .ForMember(d => d.Arquivo, m => m.Ignore())
                .ForMember(d => d.Cobranca, m => m.Ignore())
                .ForMember(d => d.Cliente, m => m.Ignore());

            CreateMap<TransferenciaBancariaDTO, TransferenciaBancaria>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoTransferencia)(int)s.Tipo))
                .ForMember(d => d.Status, m => m.Ignore())
                .ForMember(d => d.TransferenciaId, m => m.Ignore())
                .ForMember(d => d.IdentificadorTransacao, m => m.Ignore())
                .ForMember(d => d.DataSolicitacao, m => m.Ignore())
                .ForMember(d => d.DataConclusao, m => m.Ignore())
                .ForMember(d => d.MensagemErro, m => m.Ignore());
        }
    }
}
