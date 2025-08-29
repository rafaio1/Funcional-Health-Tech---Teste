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
    /// Endpoints para gestão de contas bancárias de clientes.
    /// </summary>
    /// <remarks>
    /// Autenticação: utilize o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Formato de resposta: <c>application/json</c>.
    /// </remarks>
    [ApiController]
    [Route("api/contas")]
    [Produces("application/json")]
    public class ContaController : ControllerBase
    {
        private readonly IContaRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ContaController(IContaRepository repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo; _uow = uow; _mapper = mapper;
        }

        /// <summary>
        /// Consulta uma conta pelo identificador.
        /// </summary>
        /// <param name="id">Identificador da conta.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Dados da conta.</returns>
        /// <response code="200">Conta encontrada.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Conta não encontrada.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Consultar conta por ID",
            Description = "Retorna a conta correspondente ao identificador informado."
        )]
        [ProducesResponseType(typeof(ContaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContaDTO>> GetById(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            return ent is null ? NotFound() : Ok(_mapper.Map<ContaDTO>(ent));
        }

        /// <summary>
        /// Lista contas com filtro opcional por cliente.
        /// </summary>
        /// <param name="clienteId">Identificador do cliente (opcional).</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Lista de contas.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar contas",
            Description = "Retorna contas, filtrando por cliente quando informado."
        )]
        [ProducesResponseType(typeof(List<ContaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ContaDTO>>> List(
            [FromQuery, SwaggerParameter("ID do cliente para filtrar as contas.")] long? clienteId,
            CancellationToken ct)
        {
            var list = await _repo.ListAsync(x => clienteId == null || x.ClienteId == clienteId, ct);
            return Ok(list.Select(_mapper.Map<ContaDTO>).ToList());
        }

        /// <summary>
        /// Cria uma nova conta.
        /// </summary>
        /// <param name="dto">Dados da conta a ser criada.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Identificador da conta criada.</returns>
        /// <response code="201">Conta criada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar conta",
            Description = "Cria uma nova conta e retorna o seu identificador."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Create([FromBody] ContaDTO dto, CancellationToken ct)
        {
            var ent = _mapper.Map<Conta>(dto);
            await _repo.AddAsync(ent, ct);
            await _uow.CommitAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = ent.ContaId }, ent.ContaId);
        }

        /// <summary>
        /// Atualiza uma conta existente.
        /// </summary>
        /// <param name="id">Identificador da conta.</param>
        /// <param name="dto">Dados atualizados da conta.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Conta atualizada com sucesso.</response>
        /// <response code="400">O ID do caminho difere do payload.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Conta não encontrada.</response>
        [HttpPut("{id:long}")]
        [SwaggerOperation(
            Summary = "Atualizar conta",
            Description = "Atualiza os dados de uma conta existente."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody] ContaDTO dto, CancellationToken ct)
        {
            if (id != dto.ContaId) return BadRequest("O ID do caminho difere do payload.");

            var ent = await _repo.GetByIdAsync(id, ct);
            if (ent is null) return NotFound();

            _mapper.Map(dto, ent);
            _repo.Update(ent);
            await _uow.CommitAsync(ct);

            return NoContent();
        }

        /// <summary>
        /// Exclui uma conta.
        /// </summary>
        /// <param name="id">Identificador da conta.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Conta excluída com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Conta não encontrada.</response>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(
            Summary = "Excluir conta",
            Description = "Remove a conta correspondente ao identificador informado."
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
