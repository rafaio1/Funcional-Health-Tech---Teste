using AutoMapper;
using FHT.Application.Read.DTOs;
using FHT.Domain.Entities;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using MediatR; // caso use notificações de domínio; remova se não for necessário
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
    /// Endpoints para gestão de registros de compliance.
    /// </summary>
    /// <remarks>
    /// Autenticação: utilize o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Formato de resposta: <c>application/json</c>.
    /// </remarks>
    [ApiController]
    [Route("api/compliances")]
    [Produces("application/json")]
    public class ComplianceController : ControllerBase
    {
        private readonly IComplianceRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ComplianceController(IComplianceRepository repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo; _uow = uow; _mapper = mapper;
        }

        /// <summary>
        /// Consulta um registro de compliance pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Registro de compliance correspondente.</returns>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Consultar compliance por ID",
            Description = "Retorna o registro de compliance correspondente ao identificador informado."
        )]
        [ProducesResponseType(typeof(ComplianceDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComplianceDTO>> GetById(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            return ent is null ? NotFound() : Ok(_mapper.Map<ComplianceDTO>(ent));
        }

        /// <summary>
        /// Lista registros de compliance com filtro opcional por cliente.
        /// </summary>
        /// <param name="clienteId">Identificador do cliente (opcional).</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Lista de registros de compliance.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar compliances",
            Description = "Retorna registros de compliance, filtrando por cliente quando informado."
        )]
        [ProducesResponseType(typeof(List<ComplianceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ComplianceDTO>>> List(
            [FromQuery, SwaggerParameter("ID do cliente para filtrar os registros.")] long? clienteId,
            CancellationToken ct)
        {
            var list = await _repo.ListAsync(x => clienteId == null || x.ClienteId == clienteId, ct);
            return Ok(list.Select(_mapper.Map<ComplianceDTO>).ToList());
        }

        /// <summary>
        /// Cria um novo registro de compliance.
        /// </summary>
        /// <param name="dto">Dados do registro a ser criado.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Identificador do registro criado.</returns>
        /// <response code="201">Registro criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar compliance",
            Description = "Cria um novo registro de compliance e retorna o seu identificador."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Create([FromBody] ComplianceDTO dto, CancellationToken ct)
        {
            var ent = _mapper.Map<Compliance>(dto);
            await _repo.AddAsync(ent, ct);
            await _uow.CommitAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = ent.ComplianceId }, ent.ComplianceId);
        }

        /// <summary>
        /// Atualiza um registro de compliance existente.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="dto">Dados atualizados.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Registro atualizado com sucesso.</response>
        /// <response code="400">O ID do caminho difere do payload.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpPut("{id:long}")]
        [SwaggerOperation(
            Summary = "Atualizar compliance",
            Description = "Atualiza os dados de um registro de compliance existente."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody] ComplianceDTO dto, CancellationToken ct)
        {
            if (id != dto.ComplianceId) return BadRequest("O ID do caminho difere do payload.");

            var ent = await _repo.GetByIdAsync(id, ct);
            if (ent is null) return NotFound();

            _mapper.Map(dto, ent);
            _repo.Update(ent);
            await _uow.CommitAsync(ct);

            return NoContent();
        }

        /// <summary>
        /// Exclui um registro de compliance.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Registro excluído com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(
            Summary = "Excluir compliance",
            Description = "Remove o registro de compliance correspondente ao identificador informado."
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
