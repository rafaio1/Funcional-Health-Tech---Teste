using AutoMapper;
using FHT.Application.Read.Command.Cliente;
using FHT.Application.Read.DTOs;
using FHT.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Read.Handler.Cliente
{
    public class ListarClientesHandler : IRequestHandler<ListarClientesQuery, List<ClienteDTO>>
    {
        private readonly IClienteRepository _repo;
        private readonly IMapper _mapper;

        public ListarClientesHandler(IClienteRepository repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }

        public async Task<List<ClienteDTO>> Handle(ListarClientesQuery request, CancellationToken ct)
        {
            var list = await _repo.ListAsync(x =>
                (request.Nome == null || x.Nome.Contains(request.Nome)) &&
                (request.Status == null || (int)x.Status == (int)request.Status), ct);

            return list.Select(_mapper.Map<ClienteDTO>).ToList();
        }
    }
}