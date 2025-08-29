using MediatR;

namespace FHT.Application.Read.Command.Cobranca
{
    public record CancelarCobrancaCommand(long CobrancaId, string Motivo = null) : IRequest<bool>;
}