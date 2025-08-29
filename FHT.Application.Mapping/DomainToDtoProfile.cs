using AutoMapper;
using FHT.Application.Read.DTOs;
using FHT.Domain.Entities;

namespace FHT.Application.Mapping
{
    public class DomainToDtoProfile : Profile
    {
        public DomainToDtoProfile()
        {
            CreateMap<TipoCliente, TipoClienteDTO>().ConvertUsing(src => (TipoClienteDTO)src);
            CreateMap<StatusCliente, StatusClienteDTO>().ConvertUsing(src => (StatusClienteDTO)src);

            CreateMap<TipoConta, TipoContaDTO>().ConvertUsing(src => (TipoContaDTO)src);
            CreateMap<StatusConta, StatusContaDTO>().ConvertUsing(src => (StatusContaDTO)src);

            CreateMap<StatusKyc, StatusKycDTO>().ConvertUsing(src => (StatusKycDTO)src);
            CreateMap<NivelRisco, NivelRiscoDTO>().ConvertUsing(src => (NivelRiscoDTO)src);

            CreateMap<TipoContato, TipoContatoDTO>().ConvertUsing(src => (TipoContatoDTO)src);
            CreateMap<TipoDocumentoFiscal, TipoDocumentoFiscalDTO>().ConvertUsing(src => (TipoDocumentoFiscalDTO)src);
            CreateMap<TipoEndereco, TipoEnderecoDTO>().ConvertUsing(src => (TipoEnderecoDTO)src);

            CreateMap<MetodoCobranca, MetodoCobrancaDTO>().ConvertUsing(src => (MetodoCobrancaDTO)src);
            CreateMap<SituacaoCobranca, SituacaoCobrancaDTO>().ConvertUsing(src => (SituacaoCobrancaDTO)src);

            CreateMap<AcaoAuditoria, AcaoAuditoriaDTO>().ConvertUsing(src => (AcaoAuditoriaDTO)src);

            CreateMap<Cliente, ClienteDTO>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoClienteDTO)s.Tipo))
                .ForMember(d => d.Status, m => m.MapFrom(s => (StatusClienteDTO)s.Status));

            CreateMap<Conta, ContaDTO>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoContaDTO)s.Tipo))
                .ForMember(d => d.Status, m => m.MapFrom(s => (StatusContaDTO)s.Status));

            CreateMap<Compliance, ComplianceDTO>()
                .ForMember(d => d.StatusKyc, m => m.MapFrom(s => (StatusKycDTO)s.StatusKyc))
                .ForMember(d => d.NivelRisco, m => m.MapFrom(s => (NivelRiscoDTO)s.NivelRisco));

            CreateMap<Contato, ContatoDTO>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoContatoDTO)s.Tipo));

            CreateMap<DocumentoFiscal, DocumentoFiscalDTO>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoDocumentoFiscalDTO)s.Tipo));

            CreateMap<EnderecoFiscal, EnderecoFiscalDTO>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoEnderecoDTO)s.Tipo));

            CreateMap<Societario, SocietarioDTO>();

            CreateMap<DadoPessoal, DadoPessoalDTO>();

            CreateMap<Auditoria, AuditoriaDTO>()
                .ForMember(d => d.Acao, m => m.MapFrom(s => (AcaoAuditoriaDTO)s.Acao));

            CreateMap<Cobranca, CobrancaDTO>()
                .ForMember(d => d.Metodo, m => m.MapFrom(s => (MetodoCobrancaDTO)s.Metodo))
                .ForMember(d => d.Situacao, m => m.MapFrom(s => (SituacaoCobrancaDTO)s.Situacao));

            CreateMap<Comprovante, ComprovanteDTO>();

            CreateMap<TransferenciaBancaria, TransferenciaBancariaDTO>()
                .ForMember(d => d.Tipo, m => m.MapFrom(s => (TipoTransferenciaDTO)(int)s.Tipo))
                .ForMember(d => d.Status, m => m.MapFrom(s => (StatusTransferenciaDTO)(int)s.Status));
        }
    }
}
