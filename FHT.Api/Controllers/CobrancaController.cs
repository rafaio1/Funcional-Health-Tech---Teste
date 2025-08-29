using FHT.Application.Read.DTOs;
using FHT.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FHT.Application.Read.Command.Cobranca;

namespace FHT.Api.Controllers
{
    /// <summary>
    /// Endpoints para gestão de cobranças.
    /// </summary>
    /// <remarks>
    /// Autenticação: envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Respostas: <c>application/json</c>.
    /// </remarks>
    [ApiController]
    [Route("api/cobrancas")]
    [Produces("application/json")]
    public class CobrancaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICobrancaRepository _repo;
        private readonly IMapper _mapper;

        public CobrancaController(IMediator mediator, ICobrancaRepository repo, IMapper mapper)
        {
            _mediator = mediator; _repo = repo; _mapper = mapper;
        }

        /// <summary>
        /// Busca uma cobrança pelo identificador.
        /// </summary>
        /// <param name="id">Identificador da cobrança.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Dados da cobrança.</returns>
        /// <response code="200">Cobrança encontrada.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Cobrança não encontrada.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Consultar cobrança por ID",
            Description = "Retorna a cobrança correspondente ao identificador informado."
        )]
        [ProducesResponseType(typeof(CobrancaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CobrancaDTO>> GetById(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            return ent is null ? NotFound() : Ok(_mapper.Map<CobrancaDTO>(ent));
        }

        /// <summary>
        /// Lista cobranças com filtros opcionais.
        /// </summary>
        /// <param name="clienteId">Filtra por ID de cliente.</param>
        /// <param name="situacao">Situação da cobrança.</param>
        /// <param name="pago">Filtra por estado de pagamento.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Lista de cobranças.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar cobranças",
            Description = "Retorna cobranças filtrando por cliente, situação e status de pagamento (se informados)."
        )]
        [ProducesResponseType(typeof(List<CobrancaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<CobrancaDTO>>> List(
            [FromQuery, SwaggerParameter("ID do cliente.")] long? clienteId,
            [FromQuery, SwaggerParameter("Situação da cobrança.")] SituacaoCobrancaDTO? situacao,
            [FromQuery, SwaggerParameter("Indica se a cobrança está paga.")] bool? pago,
            CancellationToken ct)
        {
            var list = await _repo.ListAsync(x =>
                    (clienteId == null || x.ClienteId == clienteId) &&
                    (situacao == null || (int)x.Situacao == (int)situacao) &&
                    (pago == null || x.Pago == pago),
                ct);

            return Ok(list.Select(_mapper.Map<CobrancaDTO>).ToList());
        }

        /// <summary>
        /// Cria uma nova cobrança.
        /// </summary>
        /// <param name="cmd">Dados para criação da cobrança.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Identificador da cobrança criada.</returns>
        /// <response code="201">Cobrança criada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Criar cobrança",
            Description = "Cria uma nova cobrança e retorna seu identificador."
        )]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Create([FromBody] CriarCobrancaCommand cmd, CancellationToken ct)
        {
            var id = await _mediator.Send(cmd, ct);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        /// <summary>
        /// Realiza o pagamento de uma cobrança.
        /// </summary>
        /// <param name="id">Identificador da cobrança.</param>
        /// <param name="body">Dados para efetuar o pagamento.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Identificador do comprovante gerado.</returns>
        /// <response code="200">Pagamento realizado e comprovante retornado.</response>
        /// <response code="400">Dados inválidos para pagamento.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Cobrança não encontrada.</response>
        [HttpPost("{id:long}/pagar")]
        [SwaggerOperation(
            Summary = "Pagar cobrança",
            Description = "Efetua o pagamento da cobrança e retorna o ID do comprovante."
        )]
        [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<long>> Pagar(long id, [FromBody] PagarCobrancaCommand body, CancellationToken ct)
        {
            var cmd = body with { CobrancaId = id };
            var comprovanteId = await _mediator.Send(cmd, ct);
            return Ok(comprovanteId);
        }

        /// <summary>
        /// Cancela uma cobrança.
        /// </summary>
        /// <param name="id">Identificador da cobrança.</param>
        /// <param name="body">Dados para cancelamento.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Nenhum conteúdo se cancelada.</returns>
        /// <response code="204">Cobrança cancelada com sucesso.</response>
        /// <response code="400">Dados inválidos para cancelamento.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Cobrança não encontrada.</response>
        [HttpPost("{id:long}/cancelar")]
        [SwaggerOperation(
            Summary = "Cancelar cobrança",
            Description = "Cancela a cobrança informada, quando possível."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Cancelar(long id, [FromBody] CancelarCobrancaCommand body, CancellationToken ct)
        {
            var cmd = body with { CobrancaId = id };
            var ok = await _mediator.Send(cmd, ct);
            return ok ? NoContent() : NotFound();
        }

        /// <summary>
        /// Obtém o comprovante de pagamento de uma cobrança.
        /// </summary>
        /// <param name="id">Identificador da cobrança.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Comprovante de pagamento.</returns>
        /// <response code="200">Comprovante encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Cobrança ou comprovante não encontrado.</response>
        [HttpGet("{id:long}/comprovante")]
        [SwaggerOperation(
            Summary = "Obter comprovante",
            Description = "Retorna o comprovante de pagamento associado à cobrança."
        )]
        [ProducesResponseType(typeof(ComprovanteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComprovanteDTO>> ObterComprovante(long id, CancellationToken ct)
        {
            var dto = await _mediator.Send(new ObterComprovantePorCobrancaQuery(id), ct);
            return dto is null ? NotFound() : Ok(dto);
        }
    }
}
