using FHT.Application.Read.DTOs;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;

namespace FHT.Application.Read.Command.Cobranca
{
    public record CriarCobrancaCommand(
        [Required] long ClienteId,
        [Required] MetodoCobrancaDTO Metodo,
        [Required] decimal Valor,
        DateTimeOffset? DataVencimento,
        string ReferenciaExterna,
        string Descricao
    ) : IRequest<long>;
}