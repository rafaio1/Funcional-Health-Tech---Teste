using AutoMapper;
using FHT.Application.Read.Command.Cliente;
using FHT.Application.Read.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Api.Controllers
{
    /// <summary>
    /// Endpoints de gerenciamento de clientes.
    /// </summary>
    /// <remarks>
    /// Autenticação: utilize o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Respostas no formato JSON.<br/>
    /// </remarks>
    [ApiController]
    [Route("api/clientes")]
    [Produces("application/json")]
    public class ClienteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClienteController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Consulta um cliente pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do cliente.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Objeto cliente correspondente ao ID informado.</returns>
        /// <response code="200">Cliente encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Cliente não encontrado.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(Summary = "Consultar cliente por ID", Description = "Retorna o cliente correspondente ao identificador informado.")]
        [ProducesResponseType(typeof(ClienteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClienteDTO>> GetById(long id, CancellationToken ct)
        {
            var dto = await _mediator.Send(new ObterClientePorIdQuery(id), ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Lista clientes com filtros opcionais.
        /// </summary>
        /// <param name="nome">Nome parcial ou completo do cliente.</param>
        /// <param name="status">Status do cliente (ex.: Ativo, Inativo).</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Lista de clientes.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Listar clientes", Description = "Retorna clientes filtrando por nome e status, se informados.")]
        [ProducesResponseType(typeof(List<ClienteDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ClienteDTO>>> List(
            [FromQuery, SwaggerParameter("Nome do cliente (parcial ou completo).")] string? nome,
            [FromQuery, SwaggerParameter("Status do cliente (Ativo/Inativo).")] StatusClienteDTO? status,
            CancellationToken ct)
        {
            var list = await _mediator.Send(new ListarClientesQuery(nome, status), ct);
            return Ok(list);
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="cmd">Dados do cliente a ser criado.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>ID do cliente criado.</returns>
        /// <response code="201">Cliente criado com sucesso.</response>
        /// <response code="400">Dados inválidos ou malformados.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar cliente", Description = "Cria um novo cliente com os dados informados.")]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Create([FromBody] CriarClienteCommand cmd, CancellationToken ct)
        {
            var id = await _mediator.Send(cmd, ct);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="id">Identificador do cliente a ser atualizado.</param>
        /// <param name="cmd">Dados atualizados do cliente.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo se atualizado, ou erro se não encontrado.</returns>
        /// <response code="204">Cliente atualizado com sucesso.</response>
        /// <response code="400">O ID informado não corresponde ao payload.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Cliente não encontrado.</response>
        [HttpPut("{id:long}")]
        [SwaggerOperation(Summary = "Atualizar cliente", Description = "Atualiza os dados de um cliente existente.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody] AtualizarClienteCommand cmd, CancellationToken ct)
        {
            if (id != cmd.ClienteId) return BadRequest("O ID do caminho difere do payload.");
            var ok = await _mediator.Send(cmd, ct);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>
        /// Remove um cliente.
        /// </summary>
        /// <param name="id">Identificador do cliente a ser removido.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo se removido, ou erro se não encontrado.</returns>
        /// <response code="204">Cliente removido com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Cliente não encontrado.</response>
        [HttpDelete("{id:long}")]
        [SwaggerOperation(Summary = "Remover cliente", Description = "Remove o cliente correspondente ao identificador informado.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(long id, CancellationToken ct)
        {
            var ok = await _mediator.Send(new ExcluirClienteCommand(id), ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
