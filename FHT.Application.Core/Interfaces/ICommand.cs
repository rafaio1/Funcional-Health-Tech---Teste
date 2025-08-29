using MediatR;

namespace FHT.Application.Core.Interfaces
{
    public interface ICommand : IRequest<CommandResult>, IBaseRequest
    {
    }
}
