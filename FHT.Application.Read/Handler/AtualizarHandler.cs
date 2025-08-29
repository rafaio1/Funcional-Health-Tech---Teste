using AutoMapper;
using FHT.Application.Read.Command;
using FHT.Domain.Repositories.Base;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler
{
    public class AtualizarHandler<TEntity, TUpdateDto> : IRequestHandler<AtualizarCommand<TUpdateDto>, bool>
        where TEntity : class
    {
        private readonly IRepository<TEntity> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AtualizarHandler(IRepository<TEntity> repo, IUnitOfWork uow, IMapper mapper)
        { _repo = repo; _uow = uow; _mapper = mapper; }

        public async Task<bool> Handle(AtualizarCommand<TUpdateDto> request, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(request.Id, ct);
            if (ent is null)
            {
                return false;
            }

            _mapper.Map(request.Dto, ent);
            _repo.Update(ent);
            await _uow.CommitAsync(ct);
            return true;
        }
    }
}