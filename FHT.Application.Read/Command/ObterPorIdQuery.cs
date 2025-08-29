using MediatR;

namespace FHT.Application.Read.Command
{
    public record ObterPorIdQuery<TKey, TDto>(TKey Id) : IRequest<TDto>;
}