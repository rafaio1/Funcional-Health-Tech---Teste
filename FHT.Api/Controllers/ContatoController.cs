using AutoMapper;
using FHT.Application.Read.DTOs;
using FHT.Domain.Entities;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Api.Controllers
{
    /// <summary>
    /// Endpoints para gestão de contatos de clientes.
    /// </summary>
    /// <remarks>
    /// Autenticação: utilize o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Formato de resposta: <c>application/json</c>.
    /// </remarks>
    [ApiController]
    [Route("api/contatos")]
    [Produces("application/json")]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ContatoController(IContatoRepository repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo; _uow = uow; _mapper = mapper;
        }

        /// <summary>
        /// Consulta um contato pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do contato.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Dados do contato.</returns>
        /// <response code="200">Contato encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Contato não encontrado.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Consultar contato por ID",
            Description = "Retorna o contato correspondente ao identificador informado."
        )]
        [ProducesResponseType(typeof(ContatoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContatoDTO>> GetById(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            return ent is null ? NotFound() : Ok(_mapper.Map<ContatoDTO>(ent));
        }

        /// <summary>
        /// Lista contatos com filtro opcional por cliente.
        /// </summary>
        /// <param name="clienteId">Identificador do cliente (opcional).</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Lista de contatos.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar contatos",
            Description = "Retorna contatos, filtrando por cliente quando informado."
        )]
        [ProducesResponseType(typeof(List<ContatoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ContatoDTO>>> List(
            [FromQuery, SwaggerParameter("ID do cliente para filtrar os contatos.")] long? clienteId,
            CancellationToken ct)
        {
            var list = await _repo.ListAsync(x => clienteId == null || x.ClienteId == clienteId, ct);
            return Ok(list.Select(_mapper.Map<ContatoDTO>).ToList());
        }

        /// <summary>
        /// Cria um novo contato.
        /// </summary>
        /// <param name="dto">Dados do contato a ser criado.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Identificador do contato criado.</returns>
        /// <response code="201">Contato criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar contato",
            Description = "Cria um novo contato e retorna o seu identificador."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Create([FromBody] ContatoDTO dto, CancellationToken ct)
        {
            var ent = _mapper.Map<Contato>(dto);
            await _repo.AddAsync(ent, ct);
            await _uow.CommitAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = ent.ContatoId }, ent.ContatoId);
        }

        /// <summary>
        /// Atualiza um contato existente.
        /// </summary>
        /// <param name="id">Identificador do contato.</param>
        /// <param name="dto">Dados atualizados do contato.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Contato atualizado com sucesso.</response>
        /// <response code="400">O ID do caminho difere do payload.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Contato não encontrado.</response>
        [HttpPut("{id:long}")]
        [SwaggerOperation(
            Summary = "Atualizar contato",
            Description = "Atualiza os dados de um contato existente."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody] ContatoDTO dto, CancellationToken ct)
        {
            if (id != dto.ContatoId) return BadRequest("O ID do caminho difere do payload.");

            var ent = await _repo.GetByIdAsync(id, ct);
            if (ent is null) return NotFound();

            _mapper.Map(dto, ent);
            _repo.Update(ent);
            await _uow.CommitAsync(ct);

            return NoContent();
        }

        /// <summary>
        /// Exclui um contato.
        /// </summary>
        /// <param name="id">Identificador do contato.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Contato excluído com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Contato não encontrado.</response>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(
            Summary = "Excluir contato",
            Description = "Remove o contato correspondente ao identificador informado."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            if (ent is null) return NotFound();

            _repo.Delete(ent);
            await _uow.CommitAsync(ct);

            return NoContent();
        }
    }
}
