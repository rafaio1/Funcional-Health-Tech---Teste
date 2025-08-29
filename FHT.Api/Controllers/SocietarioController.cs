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
    /// Endpoints para gestão de informações societárias dos clientes.
    /// </summary>
    /// <remarks>
    /// Autenticação: utilize o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Formato de resposta: <c>application/json</c>.
    /// </remarks>
    [ApiController]
    [Route("api/societarios")]
    [Produces("application/json")]
    public class SocietarioController : ControllerBase
    {
        private readonly ISocietarioRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SocietarioController(ISocietarioRepository repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo; _uow = uow; _mapper = mapper;
        }

        /// <summary>
        /// Consulta um registro societário pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do registro societário.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Dados do registro societário.</returns>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Consultar societário por ID",
            Description = "Retorna o registro societário correspondente ao identificador informado."
        )]
        [ProducesResponseType(typeof(SocietarioDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SocietarioDTO>> GetById(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            return ent is null ? NotFound() : Ok(_mapper.Map<SocietarioDTO>(ent));
        }

        /// <summary>
        /// Lista registros societários com filtro opcional por cliente.
        /// </summary>
        /// <param name="clienteId">Identificador do cliente (opcional).</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Lista de registros societários.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar societários",
            Description = "Retorna registros societários, filtrando por cliente quando informado."
        )]
        [ProducesResponseType(typeof(List<SocietarioDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<SocietarioDTO>>> List(
            [FromQuery, SwaggerParameter("ID do cliente para filtrar os registros.")] long? clienteId,
            CancellationToken ct)
        {
            var list = await _repo.ListAsync(x => clienteId == null || x.ClienteId == clienteId, ct);
            return Ok(list.Select(_mapper.Map<SocietarioDTO>).ToList());
        }

        /// <summary>
        /// Cria um novo registro societário.
        /// </summary>
        /// <param name="dto">Dados do registro a ser criado.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Identificador do registro criado.</returns>
        /// <response code="201">Registro criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar societário",
            Description = "Cria um novo registro societário e retorna o seu identificador."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Create([FromBody] SocietarioDTO dto, CancellationToken ct)
        {
            var ent = _mapper.Map<Societario>(dto);
            await _repo.AddAsync(ent, ct);
            await _uow.CommitAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = ent.SocietarioId }, ent.SocietarioId);
        }

        /// <summary>
        /// Atualiza um registro societário existente.
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
            Summary = "Atualizar societário",
            Description = "Atualiza os dados de um registro societário existente."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody] SocietarioDTO dto, CancellationToken ct)
        {
            if (id != dto.SocietarioId) return BadRequest("O ID do caminho difere do payload.");

            var ent = await _repo.GetByIdAsync(id, ct);
            if (ent is null) return NotFound();

            _mapper.Map(dto, ent);
            _repo.Update(ent);
            await _uow.CommitAsync(ct);

            return NoContent();
        }

        /// <summary>
        /// Exclui um registro societário.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Registro excluído com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(
            Summary = "Excluir societário",
            Description = "Remove o registro societário correspondente ao identificador informado."
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
