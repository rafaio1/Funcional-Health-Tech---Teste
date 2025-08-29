using AutoMapper;
using FHT.Application.Read.DTOs;
using FHT.Domain.Entities;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Api.Controllers
{
    /// <summary>
    /// Endpoints para gestão de dados pessoais de clientes.
    /// </summary>
    /// <remarks>
    /// Autenticação: utilize o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Respostas: <c>application/json</c>.<br/>
    /// Observação: recursos tratados como dados pessoais devem seguir suas políticas de privacidade e LGPD.
    /// </remarks>
    [ApiController]
    [Route("api/dados-pessoais")]
    [Produces("application/json")]
    public class DadoPessoalController : ControllerBase
    {
        private readonly IDadoPessoalRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DadoPessoalController(IDadoPessoalRepository repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo; _uow = uow; _mapper = mapper;
        }

        /// <summary>
        /// Consulta dados pessoais pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do registro de dados pessoais.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Registro de dados pessoais correspondente.</returns>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Consultar dados pessoais por ID",
            Description = "Retorna o registro de dados pessoais correspondente ao identificador informado."
        )]
        [ProducesResponseType(typeof(DadoPessoalDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DadoPessoalDTO>> GetById(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            return ent is null ? NotFound() : Ok(_mapper.Map<DadoPessoalDTO>(ent));
        }

        /// <summary>
        /// Consulta dados pessoais pelo identificador do cliente.
        /// </summary>
        /// <param name="clienteId">Identificador do cliente.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Registro de dados pessoais vinculado ao cliente.</returns>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Registro não encontrado para o cliente informado.</response>
        [HttpGet("por-cliente/{clienteId:long}")]
        [SwaggerOperation(
            Summary = "Consultar dados pessoais por cliente",
            Description = "Retorna o registro de dados pessoais vinculado ao ID do cliente."
        )]
        [ProducesResponseType(typeof(DadoPessoalDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DadoPessoalDTO>> GetByCliente(long clienteId, CancellationToken ct)
        {
            var ent = await _repo.FirstOrDefaultAsync(x => x.ClienteId == clienteId, ct);
            return ent is null ? NotFound() : Ok(_mapper.Map<DadoPessoalDTO>(ent));
        }

        /// <summary>
        /// Cria um novo registro de dados pessoais.
        /// </summary>
        /// <param name="dto">Dados do registro a ser criado.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Identificador do registro criado.</returns>
        /// <response code="201">Registro criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar dados pessoais",
            Description = "Cria um novo registro de dados pessoais e retorna o seu identificador."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Create([FromBody] DadoPessoalDTO dto, CancellationToken ct)
        {
            var ent = _mapper.Map<DadoPessoal>(dto);
            await _repo.AddAsync(ent, ct);
            await _uow.CommitAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = ent.DadoPessoalId }, ent.DadoPessoalId);
        }

        /// <summary>
        /// Atualiza um registro de dados pessoais existente.
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
            Summary = "Atualizar dados pessoais",
            Description = "Atualiza os dados de um registro de dados pessoais existente."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody] DadoPessoalDTO dto, CancellationToken ct)
        {
            if (id != dto.DadoPessoalId) return BadRequest("O ID do caminho difere do payload.");

            var ent = await _repo.GetByIdAsync(id, ct);
            if (ent is null) return NotFound();

            _mapper.Map(dto, ent);
            _repo.Update(ent);
            await _uow.CommitAsync(ct);

            return NoContent();
        }

        /// <summary>
        /// Exclui um registro de dados pessoais.
        /// </summary>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Registro excluído com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(
            Summary = "Excluir dados pessoais",
            Description = "Remove o registro de dados pessoais correspondente ao identificador informado."
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
