using AutoMapper;
using FHT.Application.Read.Command;
using FHT.Domain.Repositories.Base;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler
{
    public class CriarHandler<TEntity, TCreateDto, TKey> : IRequestHandler<CriarCommand<TCreateDto, TKey>, TKey>
        where TEntity : class, new()
    {
        private readonly IRepository<TEntity> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CriarHandler(IRepository<TEntity> repo, IUnitOfWork uow, IMapper mapper)
        { _repo = repo; _uow = uow; _mapper = mapper; }

        public async Task<TKey> Handle(CriarCommand<TCreateDto, TKey> request, CancellationToken ct)
        {
            TEntity ent = _mapper.Map<TEntity>(request.Dto);
            await _repo.AddAsync(ent, ct);
            await _uow.CommitAsync(ct);

            TKey id = (TKey)typeof(TEntity).GetProperty("Id")?.GetValue(ent)
                     ?? (TKey)typeof(TEntity).GetProperty($"{typeof(TEntity).Name}Id")?.GetValue(ent);
            return id;
        }
    }
}