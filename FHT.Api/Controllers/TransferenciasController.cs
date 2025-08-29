using AutoMapper;
using FHT.Application.Read.DTOs;
using FHT.Application.Read.Transferencia;
using FHT.Domain.Repositories;
using FHT.Infra.Data.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Api.Controllers
{
    /// <summary>
    /// Endpoints para criação e consulta de transferências bancárias.
    /// </summary>
    /// <remarks>
    /// Autenticação obrigatória: utilize o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Respostas: <c>application/json</c>.
    /// </remarks>
    [Authorize]
    [ApiController]
    [Route("api/transferencias")]
    [Produces("application/json")]
    public class TransferenciasController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAspNetUser _user;
        private readonly ITransferenciaRepository _repo;
        private readonly IMapper _mapper;

        public TransferenciasController(IMediator mediator, IAspNetUser user, ITransferenciaRepository repo, IMapper mapper)
        {
            _mediator = mediator;
            _user = user;
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria uma transferência bancária e debita o saldo.
        /// </summary>
        /// <param name="req">Dados da transferência a ser criada.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Transferência criada.</returns>
        /// <response code="201">Transferência criada com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpPost]
        [Consumes("application/json")]
        [SwaggerOperation(
            Summary = "Criar transferência",
            Description = "Cria uma transferência (PIX, TED/DOC, boleto, etc.) e retorna o recurso criado."
        )]
        [ProducesResponseType(typeof(TransferenciaBancariaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Criar([FromBody, Required] TransferenciaBancariaDTO req, CancellationToken ct)
        {
            if (req is null) return BadRequest("Payload ausente.");

            // Converte enum do DTO para enum do domínio
            var tipoDomain = (FHT.Domain.Entities.TipoTransferencia)(int)req.Tipo;

            var dto = await _mediator.Send(new CriarTransferenciaCommand(
                req.ClienteId, req.ContaId, tipoDomain, req.Valor, req.Descricao,
                req.PixChave,
                req.BancoDestino, req.AgenciaDestino, req.ContaDestino,
                req.DocumentoTitularDestino, req.NomeTitularDestino,
                req.CodigoBarras, req.LinhaDigitavel,
                _user.GetUserId()
            ), ct);

            var saida = _mapper.Map<TransferenciaBancariaDTO>(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = saida.TransferenciaId }, saida);
        }

        /// <summary>
        /// Consulta uma transferência pelo identificador.
        /// </summary>
        /// <param name="id">Identificador da transferência.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Dados da transferência.</returns>
        /// <response code="200">Transferência encontrada.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Transferência não encontrada.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Consultar transferência por ID",
            Description = "Retorna a transferência correspondente ao identificador informado."
        )]
        [ProducesResponseType(typeof(TransferenciaBancariaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            if (ent is null) return NotFound();
            return Ok(_mapper.Map<TransferenciaBancariaDTO>(ent));
        }
    }
}
