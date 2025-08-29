using FHT.Application.Read.Command.Cliente;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler.Cliente
{
    public class CriarClienteHandler : IRequestHandler<CriarClienteCommand, long>
    {
        private readonly IClienteRepository _repo;
        private readonly IUnitOfWork _uow;

        public CriarClienteHandler(IClienteRepository repo, IUnitOfWork uow)
        {
            _repo = repo; _uow = uow;
        }

        public async Task<long> Handle(CriarClienteCommand request, CancellationToken ct)
        {
            Domain.Entities.Cliente ent = new Domain.Entities.Cliente
            {
                Nome = request.Nome,
                Tipo = (Domain.Entities.TipoCliente)request.Tipo,
                Status = Domain.Entities.StatusCliente.Ativo
            };

            await _repo.AddAsync(ent, ct);
            await _uow.CommitAsync(ct);
            return ent.ClienteId;
        }
    }
}