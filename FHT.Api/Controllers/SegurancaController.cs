using FHT.Api.Config.Jwt;
using FHT.Api.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FHT.Api.Controllers
{
    /// <summary>
    /// Endpoints de segurança/autenticação.
    /// </summary>
    /// <remarks>
    /// Fluxo:
    /// 1) Envie <c>POST /api/seguranca/login</c> com corpo JSON contendo usuário e senha.<br/>
    /// 2) Inclua na requisição o cabeçalho <c>Authorization: Basic base64(Auth:yyyyMMdd:FHT)</c> (conforme <see cref="BasicGateAttribute"/>).<br/>
    /// 3) Use o <c>access_token</c> retornado como <c>Bearer</c> nos demais endpoints protegidos.
    /// </remarks>
    [ApiController]
    [Route("api/seguranca")]
    [Produces("application/json")]
    public class SegurancaController : ControllerBase
    {
        private readonly AppJwtSettings _jwt;

        public SegurancaController(IOptions<AppJwtSettings> jwtOptions)
            => _jwt = jwtOptions.Value;

        /// <summary>
        /// Corpo da requisição de login.
        /// </summary>
        public record LoginRequest(string Usuario, string Senha);

        /// <summary>
        /// Resposta de autenticação com token JWT.
        /// </summary>
        public record TokenResponse(string access_token, string token_type, int expires_in);

        /// <summary>
        /// Autentica as credenciais e retorna um token JWT.
        /// </summary>
        /// <param name="req">Credenciais de acesso.</param>
        /// <returns>Token JWT, tipo e tempo de expiração em segundos.</returns>
        /// <response code="200">Credenciais válidas. Token emitido.</response>
        /// <response code="401">Não autorizado. Credenciais inválidas ou cabeçalho Basic ausente/inválido (Basic base64(Auth:yyyyMMdd:FHT)).</response>
        /// <response code="422">Payload inválido ou malformado.</response>
        [HttpPost("login")]
        [AllowAnonymous]
#if !DEBUG
        [BasicGate]
#endif
        [Consumes("application/json")]
        [SwaggerOperation(
            Summary = "Autentica e retorna um JWT",
            Description =
                "Requer o cabeçalho **Authorization: Basic base64(Auth:yyyyMMdd:FHT)** (validação via BasicGate). " +
                "Retorna um **access_token** para uso como **Bearer** nos demais endpoints."
        )]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (req is null)
                return UnprocessableEntity("Payload ausente.");

            if (!req.Usuario.Equals("FHT") || !req.Senha.Equals("FHT"))
                return Unauthorized(new { error = "Credenciais inválidas." });

            var expiresUtc = DateTime.UtcNow.AddHours(_jwt.Expiration);
            var token = GenerateJwt(userId: "FHT", userName: "FHT Admin", expiresUtc);

            var response = new TokenResponse(
                access_token: token,
                token_type: "Bearer",
                expires_in: (int)TimeSpan.FromHours(_jwt.Expiration).TotalSeconds
            );

            return Ok(response);
        }

        private string GenerateJwt(string userId, string userName, DateTime expiresUtc)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
            };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresUtc,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
