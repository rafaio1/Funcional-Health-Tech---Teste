using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FHT.Domain.Entities;

namespace FHT.Infra.Data.Context
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrationsWithAudit(this IServiceProvider services, ILogger logger = null)
        {
            using var scope = services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var pending = ctx.Database.GetPendingMigrations().ToList();
            if (pending.Count == 0)
            {
                logger?.LogInformation("[DB] Sem migrations pendentes.");
                return;
            }

            logger?.LogInformation("[DB] Aplicando {Count} migrations pendentes: {Migs}", pending.Count, string.Join(", ", pending));

            ctx.Database.Migrate();

            try
            {
                var auditoria = new Auditoria
                {
                    Entidade = "__MIGRATIONS__",
                    EntidadeId = string.Join(",", pending),
                    Acao = AcaoAuditoria.MIGRACAO, 
                    Motivo = "Migrations aplicadas automaticamente na inicialização da API.",
                    UsuarioId = null,
                    UsuarioLogin = "master", 
                    CorrelacaoId = Guid.NewGuid().ToString("N"),
                    SessionId = null,
                    OrigemIp = null,
                    UserAgent = "FHT.Api/Startup",
                    DadosAntes = null,
                    DadosDepois = null,
                    Sucesso = true,
                    ErroMsg = null,
                    DataEvento = DateTimeOffset.UtcNow,
                    DataInsercao = DateTimeOffset.UtcNow
                };

                ctx.Auditorias.Add(auditoria);
                ctx.SaveChanges();

                logger?.LogInformation("[DB] Migrations aplicadas e registradas na AUDITORIA.");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "[DB] Falha ao registrar auditoria de migrations.");
            }
        }
    }
}
