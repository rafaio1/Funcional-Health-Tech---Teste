using FHT.Domain.Entities;
using MediatR;

namespace FHT.Application.Read.Transferencia
{
    public record CriarTransferenciaCommand(
        long ClienteId,
        long ContaId,
        TipoTransferencia Tipo,
        decimal Valor,
        string Descricao,
        string PixChave,
        string BancoDestino, string AgenciaDestino, string ContaDestino,
        string DocumentoTitularDestino, string NomeTitularDestino,
        string CodigoBarras, string LinhaDigitavel,
        long UsuarioId
    ) : IRequest<long>;
}
