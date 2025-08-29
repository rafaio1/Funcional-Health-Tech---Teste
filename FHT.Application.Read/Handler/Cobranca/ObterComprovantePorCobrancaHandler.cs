using AutoMapper;
using FHT.Application.Read.Command.Cobranca;
using FHT.Application.Read.DTOs;
using FHT.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler.Cobranca
{
    public class ObterComprovantePorCobrancaHandler : IRequestHandler<ObterComprovantePorCobrancaQuery, ComprovanteDTO>
    {
        private readonly IComprovanteRepository _repo;
        private readonly IMapper _mapper;

        public ObterComprovantePorCobrancaHandler(IComprovanteRepository repo, IMapper mapper)
        { _repo = repo; _mapper = mapper; }

        public async Task<ComprovanteDTO> Handle(ObterComprovantePorCobrancaQuery request, CancellationToken ct)
        {
            var comp = await _repo.ListAsync(c => c.CobrancaId == request.CobrancaId, ct);
            return comp is null ? null : _mapper.Map<ComprovanteDTO>(comp);
        }
    }
}