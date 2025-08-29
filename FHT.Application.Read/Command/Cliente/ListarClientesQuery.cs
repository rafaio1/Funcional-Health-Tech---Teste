using FHT.Application.Read.DTOs;
using MediatR;
using System.Collections.Generic;

namespace FHT.Application.Read.Command.Cliente
{
    public record ListarClientesQuery(string Nome = null, StatusClienteDTO? Status = null)
        : IRequest<List<ClienteDTO>>;
}