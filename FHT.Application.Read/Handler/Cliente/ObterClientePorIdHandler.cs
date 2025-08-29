using AutoMapper;
using FHT.Application.Read.Command.Cliente;
using FHT.Application.Read.DTOs;
using FHT.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler.Cliente
{
    public class ObterClientePorIdHandler : IRequestHandler<ObterClientePorIdQuery, ClienteDTO>
    {
        private readonly IClienteRepository _repo;
        private readonly IMapper _mapper;

        public ObterClientePorIdHandler(IClienteRepository repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }

        public async Task<ClienteDTO> Handle(ObterClientePorIdQuery request, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(request.ClienteId, ct);
            return ent is null ? null : _mapper.Map<ClienteDTO>(ent);
        }
    }
}