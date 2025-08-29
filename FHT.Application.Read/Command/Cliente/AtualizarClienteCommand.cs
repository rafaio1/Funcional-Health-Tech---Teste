using FHT.Application.Read.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FHT.Application.Read.Command.Cliente
{
    public record AtualizarClienteCommand(
        [Required] long ClienteId,
        string Nome,
        StatusClienteDTO? Status
    ) : IRequest<bool>;
}