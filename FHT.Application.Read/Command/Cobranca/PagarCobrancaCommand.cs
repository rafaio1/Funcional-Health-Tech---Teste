using MediatR;

namespace FHT.Application.Read.Command.Cobranca
{
    public record PagarCobrancaCommand(
        long CobrancaId,
        decimal? ValorPago = null,
        string IdentificadorTransacao = null,
        string Emissor = null
    ) : IRequest<long>; 
}