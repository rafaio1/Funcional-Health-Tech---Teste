using MediatR;

namespace FHT.Application.Read.Command
{
    public record ExcluirCommand(long Id) : IRequest<bool>;
}