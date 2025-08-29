using AutoMapper;
using FHT.Application.Read.DTOs;
using FHT.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Api.Controllers
{
    /// <summary>
    /// Endpoints para consulta de comprovantes.
    /// </summary>
    /// <remarks>
    /// Autenticação: utilize o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.<br/>
    /// Respostas: <c>application/json</c>.
    /// </remarks>
    [ApiController]
    [Route("api/comprovantes")]
    [Produces("application/json")]
    public class ComprovanteController : ControllerBase
    {
        private readonly IComprovanteRepository _repo;
        private readonly IMapper _mapper;

        public ComprovanteController(IComprovanteRepository repo, IMapper mapper)
        {
            _repo = repo; _mapper = mapper;
        }

        /// <summary>
        /// Consulta um comprovante pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do comprovante.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Comprovante correspondente ao ID informado.</returns>
        /// <response code="200">Comprovante encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Comprovante não encontrado.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Consultar comprovante por ID",
            Description = "Retorna o comprovante correspondente ao identificador informado."
        )]
        [ProducesResponseType(typeof(ComprovanteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComprovanteDTO>> GetById(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            return ent is null ? NotFound() : Ok(_mapper.Map<ComprovanteDTO>(ent));
        }

        /// <summary>
        /// Consulta o comprovante associado a uma cobrança.
        /// </summary>
        /// <param name="cobrancaId">Identificador da cobrança.</param>
        /// <param name="ct">Token de cancelamento.</param>
        /// <returns>Comprovante vinculado à cobrança informada.</returns>
        /// <response code="200">Comprovante encontrado.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Comprovante não encontrado para a cobrança informada.</response>
        [HttpGet("por-cobranca/{cobrancaId:long}")]
        [SwaggerOperation(
            Summary = "Consultar comprovante por cobrança",
            Description = "Retorna o comprovante vinculado ao ID de uma cobrança."
        )]
        [ProducesResponseType(typeof(ComprovanteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ComprovanteDTO>> GetByCobranca(long cobrancaId, CancellationToken ct)
        {
            // Considerando relação 1:1 entre cobrança e comprovante.
            var ent = (await _repo.ListAsync(x => x.CobrancaId == cobrancaId, ct)).FirstOrDefault();
            return ent is null ? NotFound() : Ok(_mapper.Map<ComprovanteDTO>(ent));
        }
    }
}
