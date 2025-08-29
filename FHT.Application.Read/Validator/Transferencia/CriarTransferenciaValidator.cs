using FHT.Application.Read.DTOs;
using FluentValidation;

namespace FHT.Application.Read.DTOs.Validators
{
    public class TransferenciaCreateValidator : AbstractValidator<TransferenciaBancariaDTO>
    {
        public TransferenciaCreateValidator()
        {
            RuleFor(x => x.ClienteId).GreaterThan(0);
            RuleFor(x => x.ContaId).GreaterThan(0);
            RuleFor(x => x.Valor).GreaterThan(0);

            When(x => x.Tipo == TipoTransferenciaDTO.Pix, () =>
                RuleFor(x => x.PixChave).NotEmpty());

            When(x => x.Tipo == TipoTransferenciaDTO.Ted || x.Tipo == TipoTransferenciaDTO.Doc, () =>
            {
                RuleFor(x => x.BancoDestino).NotEmpty();
                RuleFor(x => x.AgenciaDestino).NotEmpty();
                RuleFor(x => x.ContaDestino).NotEmpty();
                RuleFor(x => x.DocumentoTitularDestino).NotEmpty();
                RuleFor(x => x.NomeTitularDestino).NotEmpty();
            });

            When(x => x.Tipo == TipoTransferenciaDTO.Boleto, () =>
                RuleFor(x => x.LinhaDigitavel).NotEmpty());
        }
    }
}
