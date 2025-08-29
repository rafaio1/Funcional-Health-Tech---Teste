using MediatR;

namespace FHT.Application.Read.Command.Cliente
{
    public record ExcluirClienteCommand(long ClienteId) : IRequest<bool>;
}