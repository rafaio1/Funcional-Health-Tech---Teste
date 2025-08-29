using FHT.Application.Read.DTOs;
using MediatR;

namespace FHT.Application.Read.Command.Cobranca
{
    public record ObterComprovantePorCobrancaQuery(long CobrancaId) : IRequest<ComprovanteDTO>;
}