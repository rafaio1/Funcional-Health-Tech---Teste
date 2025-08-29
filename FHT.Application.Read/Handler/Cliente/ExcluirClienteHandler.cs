using FHT.Application.Read.Command.Cliente;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler.Cliente
{
    public class ExcluirClienteHandler : IRequestHandler<ExcluirClienteCommand, bool>
    {
        private readonly IClienteRepository _repo;
        private readonly IUnitOfWork _uow;

        public ExcluirClienteHandler(IClienteRepository repo, IUnitOfWork uow)
        {
            _repo = repo; _uow = uow;
        }

        public async Task<bool> Handle(ExcluirClienteCommand request, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(request.ClienteId, ct);
            if (ent is null)
            {
                return false;
            }

            _repo.Delete(ent);
            await _uow.CommitAsync(ct);
            return true;
        }
    }
}