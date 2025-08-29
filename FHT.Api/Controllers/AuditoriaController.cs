using AutoMapper;
using FHT.Application.Read.DTOs;
using FHT.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Api.Controllers
{
    /// <summary>
    /// Endpoints para consulta de auditorias (trilhas de eventos) da aplicação.
    /// </summary>
    /// <remarks>
    /// Autenticação: envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c> obtido no endpoint de segurança.<br/>
    /// Formato de resposta: <c>application/json</c>.<br/>
    /// Observação: os filtros por data aceitam <see cref="DateTimeOffset"/> em ISO 8601 (ex.: <c>2025-08-29T00:00:00-03:00</c>).
    /// </remarks>
    [ApiController]
    [Route("api/auditorias")]
    [Produces("application/json")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaRepository _repo;
        private readonly IMapper _mapper;

        public AuditoriaController(IAuditoriaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Busca uma auditoria pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador numérico da auditoria.</param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <returns>Objeto de auditoria correspondente ao ID informado.</returns>
        /// <response code="200">Auditoria encontrada e retornada com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        /// <response code="404">Não existe auditoria com o <paramref name="id"/> informado.</response>
        /// <response code="422">Parâmetros inválidos ou malformados.</response>
        [HttpGet("{id:long}")]
        [SwaggerOperation(
            Summary = "Buscar auditoria por ID",
            Description = "Retorna a auditoria específica informada pelo ID. Envie o cabeçalho **Authorization: Bearer &lt;token&gt;**."
        )]
        [ProducesResponseType(typeof(AuditoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<AuditoriaDTO>> GetById(long id, CancellationToken ct)
        {
            var ent = await _repo.GetByIdAsync(id, ct);
            if (ent is null) return NotFound();

            return Ok(_mapper.Map<AuditoriaDTO>(ent));
        }

        /// <summary>
        /// Lista auditorias com filtros opcionais.
        /// </summary>
        /// <param name="entidade">
        /// Nome da entidade auditada (ex.: <c>Conta</c>, <c>Cliente</c>). O filtro é exato (case-sensitive depende da implementação do repositório).
        /// </param>
        /// <param name="entidadeId">
        /// Identificador da entidade (ex.: ID da Conta). O filtro é exato.
        /// </param>
        /// <param name="usuarioLogin">
        /// Login do usuário que executou a ação (ex.: <c>rafael.silva</c>).
        /// </param>
        /// <param name="de">
        /// Data/hora inicial do período em ISO 8601 (ex.: <c>2025-08-01T00:00:00-03:00</c>).
        /// </param>
        /// <param name="ate">
        /// Data/hora final do período em ISO 8601 (ex.: <c>2025-08-31T23:59:59-03:00</c>). Inclusivo na consulta.
        /// </param>
        /// <param name="ct">Token de cancelamento da requisição.</param>
        /// <remarks>
        /// Todos os parâmetros são opcionais; quando omitidos, não filtram o resultado.<br/>
        /// Regras de filtro aplicadas:
        /// <list type="bullet">
        ///   <item><description><c>entidade</c>: <c>x =&gt; x.Entidade == entidade</c></description></item>
        ///   <item><description><c>entidadeId</c>: <c>x =&gt; x.EntidadeId == entidadeId</c></description></item>
        ///   <item><description><c>usuarioLogin</c>: <c>x =&gt; x.UsuarioLogin == usuarioLogin</c></description></item>
        ///   <item><description><c>de</c>: <c>x =&gt; x.DataEvento &gt;= de</c></description></item>
        ///   <item><description><c>ate</c>: <c>x =&gt; x.DataEvento &lt;= ate</c></description></item>
        /// </list>
        /// </remarks>
        /// <returns>Lista de auditorias no formato <see cref="AuditoriaDTO"/>.</returns>
        /// <response code="200">Lista retornada com sucesso.</response>
        /// <response code="401">Requisição não autenticada. Envie o cabeçalho <c>Authorization: Bearer &lt;token&gt;</c>.</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar auditorias",
            Description = "Retorna uma lista de auditorias filtrando por entidade, entidadeId, usuário e período. Envie o cabeçalho **Authorization: Bearer &lt;token&gt;**."
        )]
        [ProducesResponseType(typeof(List<AuditoriaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<AuditoriaDTO>>> List(
            [FromQuery, SwaggerParameter("Nome exato da entidade (ex.: Conta, Cliente).")] string? entidade,
            [FromQuery, SwaggerParameter("Identificador exato da entidade (ex.: ID da conta).")] string? entidadeId,
            [FromQuery, SwaggerParameter("Login do usuário que executou a ação (ex.: rafael.silva).")] string? usuarioLogin,
            [FromQuery, SwaggerParameter("Data/hora inicial em ISO 8601 (ex.: 2025-08-01T00:00:00-03:00).")] DateTimeOffset? de,
            [FromQuery, SwaggerParameter("Data/hora final em ISO 8601 (ex.: 2025-08-31T23:59:59-03:00).")] DateTimeOffset? ate,
            CancellationToken ct = default)
        {
            if (de.HasValue && ate.HasValue && de > ate)
                return UnprocessableEntity("O parâmetro 'de' deve ser menor ou igual a 'ate'.");

            var list = await _repo.ListAsync(
                x =>
                    (entidade == null || x.Entidade == entidade) &&
                    (entidadeId == null || x.EntidadeId == entidadeId) &&
                    (usuarioLogin == null || x.UsuarioLogin == usuarioLogin) &&
                    (de == null || x.DataEvento >= de) &&
                    (ate == null || x.DataEvento <= ate),
                ct,
                noTracking: true 
            );

            return Ok(list.Select(_mapper.Map<AuditoriaDTO>).ToList());
        }
    }
}
