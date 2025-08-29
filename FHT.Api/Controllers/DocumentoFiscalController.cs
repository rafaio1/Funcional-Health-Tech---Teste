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
    /// Endpoints para gestão de documentos fiscais.
    /// </summary>
    /// <remarks>
    /// Autenticação: utilize o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Formato de resposta: <c>application/json</c>.
    /// </remarks>
    [ApiController]
    [Route("api/documentos-fiscais")]
    [Produces("application/json")]
    public class DocumentoFiscalController : ControllerBase
    {
        private readonly IDocumentoFiscalRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DocumentoFiscalController(IDocumentoFiscalRepository repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo; _uow = uow; _mapper = mapper;
        }

        /// <summary>
        /// Consulta um documento fiscal pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do documento fiscal.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Dados do documento fiscal.</returns>
        /// <response code="200">Documento fiscal encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Documento fiscal não encontrado.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Consultar documento fiscal por ID",
            Description = "Retorna o documento fiscal correspondente ao identificador informado."
        )]
        [ProducesResponseType(typeof(DocumentoFiscalDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DocumentoFiscalDTO>> GetById(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            return ent is null ? NotFound() : Ok(_mapper.Map<DocumentoFiscalDTO>(ent));
        }

        /// <summary>
        /// Lista documentos fiscais com filtro opcional por cliente.
        /// </summary>
        /// <param name="clienteId">Identificador do cliente (opcional).</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Lista de documentos fiscais.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar documentos fiscais",
            Description = "Retorna documentos fiscais, filtrando por cliente quando informado."
        )]
        [ProducesResponseType(typeof(List<DocumentoFiscalDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<DocumentoFiscalDTO>>> List(
            [FromQuery, SwaggerParameter("ID do cliente para filtrar os documentos.")] long? clienteId,
            CancellationToken ct)
        {
            var list = await _repo.ListAsync(x => clienteId == null || x.ClienteId == clienteId, ct);
            return Ok(list.Select(_mapper.Map<DocumentoFiscalDTO>).ToList());
        }

        /// <summary>
        /// Cria um novo documento fiscal.
        /// </summary>
        /// <param name="dto">Dados do documento fiscal a ser criado.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Identificador do documento fiscal criado.</returns>
        /// <response code="201">Documento fiscal criado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar documento fiscal",
            Description = "Cria um novo documento fiscal e retorna o seu identificador."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Create([FromBody] DocumentoFiscalDTO dto, CancellationToken ct)
        {
            var ent = _mapper.Map<DocumentoFiscal>(dto);
            await _repo.AddAsync(ent, ct);
            await _uow.CommitAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = ent.DocumentoFiscalId }, ent.DocumentoFiscalId);
        }

        /// <summary>
        /// Atualiza um documento fiscal existente.
        /// </summary>
        /// <param name="id">Identificador do documento fiscal.</param>
        /// <param name="dto">Dados atualizados do documento fiscal.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Documento fiscal atualizado com sucesso.</response>
        /// <response code="400">O ID do caminho difere do payload.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Documento fiscal não encontrado.</response>
        [HttpPut("{id:long}")]
        [SwaggerOperation(
            Summary = "Atualizar documento fiscal",
            Description = "Atualiza os dados de um documento fiscal existente."
        )]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody] DocumentoFiscalDTO dto, CancellationToken ct)
        {
            if (id != dto.DocumentoFiscalId) return BadRequest("O ID do caminho difere do payload.");

            var ent = await _repo.GetByIdAsync(id, ct);
            if (ent is null) return NotFound();

            _mapper.Map(dto, ent);
            _repo.Update(ent);
            await _uow.CommitAsync(ct);

            return NoContent();
        }

        /// <summary>
        /// Exclui um documento fiscal.
        /// </summary>
        /// <param name="id">Identificador do documento fiscal.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo em caso de sucesso.</returns>
        /// <response code="204">Documento fiscal excluído com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Documento fiscal não encontrado.</response>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(
            Summary = "Excluir documento fiscal",
            Description = "Remove o documento fiscal correspondente ao identificador informado."
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
