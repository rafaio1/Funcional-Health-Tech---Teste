using FHT.Application.Read.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FHT.Application.Read.Command.Cliente
{
    public record CriarClienteCommand(
        [Required] string Nome,
        [Required] TipoClienteDTO Tipo
    ) : IRequest<long>; 
}