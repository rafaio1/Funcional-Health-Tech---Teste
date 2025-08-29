using MediatR;

namespace FHT.Application.Read.Command
{
    public record AtualizarCommand<TUpdateDto>(long Id, TUpdateDto Dto) : IRequest<bool>;
}