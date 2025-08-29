using FHT.Application.Read.Command.Cliente;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler.Cliente
{
    public class AtualizarClienteHandler : IRequestHandler<AtualizarClienteCommand, bool>
    {
        private readonly IClienteRepository _repo;
        private readonly IUnitOfWork _uow;

        public AtualizarClienteHandler(IClienteRepository repo, IUnitOfWork uow)
        {
            _repo = repo; _uow = uow;
        }

        public async Task<bool> Handle(AtualizarClienteCommand request, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(request.ClienteId, ct);
            if (ent is null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(request.Nome))
            {
                ent.Nome = request.Nome;
            }

            if (request.Status.HasValue)
            {
                ent.Status = (Domain.Entities.StatusCliente)request.Status.Value;
            }

            ent.DataAtualizacao = System.DateTimeOffset.Now;

            _repo.Update(ent);
            await _uow.CommitAsync(ct);
            return true;
        }
    }
}