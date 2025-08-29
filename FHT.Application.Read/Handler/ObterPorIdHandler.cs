using AutoMapper;
using FHT.Application.Read.Command;
using FHT.Domain.Repositories.Base;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler
{
    public class ObterPorIdHandler<TEntity, TKey, TDto> : IRequestHandler<ObterPorIdQuery<TKey, TDto>, TDto>
        where TEntity : class
    {
        private readonly IRepository<TEntity> _repo;
        private readonly IMapper _mapper;
        public ObterPorIdHandler(IRepository<TEntity> repo, IMapper mapper)
        { _repo = repo; _mapper = mapper; }

        public async Task<TDto> Handle(ObterPorIdQuery<TKey, TDto> request, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync((long)(object)request.Id!, ct); 
            return ent is null ? default : _mapper.Map<TDto>(ent);
        }
    }
}