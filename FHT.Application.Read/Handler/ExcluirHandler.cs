using FHT.Application.Read.Command;
using FHT.Domain.Repositories.Base;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler
{
    public class ExcluirHandler<TEntity> : IRequestHandler<ExcluirCommand, bool>
        where TEntity : class
    {
        private readonly IRepository<TEntity> _repo;
        private readonly IUnitOfWork _uow;

        public ExcluirHandler(IRepository<TEntity> repo, IUnitOfWork uow)
        { _repo = repo; _uow = uow; }

        public async Task<bool> Handle(ExcluirCommand request, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(request.Id, ct);
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