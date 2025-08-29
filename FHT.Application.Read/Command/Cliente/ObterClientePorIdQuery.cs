using FHT.Application.Read.DTOs;
using MediatR;

namespace FHT.Application.Read.Command.Cliente
{
    public record ObterClientePorIdQuery(long ClienteId) : IRequest<ClienteDTO>;
}