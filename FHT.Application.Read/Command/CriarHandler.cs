using MediatR;

namespace FHT.Application.Read.Command
{
    public record CriarCommand<TCreateDto, TKey>(TCreateDto Dto) : IRequest<TKey>;
}