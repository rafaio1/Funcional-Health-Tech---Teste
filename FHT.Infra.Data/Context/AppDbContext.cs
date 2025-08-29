using FHT.Domain.Entities;
using FHT.Infra.Data.Authorization;
using FHT.Infra.Data.EntityMappingConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Infra.Data.Context
{
    public class AppDbContext : DbContext
    {
        private readonly IAspNetUser? _user;

        public DbSet<Auditoria> Auditorias => Set<Auditoria>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Conta> Contas => Set<Conta>();
        public DbSet<Compliance> Compliances => Set<Compliance>();
        public DbSet<Contato> Contatos => Set<Contato>();
        public DbSet<DocumentoFiscal> DocumentosFiscais => Set<DocumentoFiscal>();
        public DbSet<EnderecoFiscal> EnderecosFiscais => Set<EnderecoFiscal>();
        public DbSet<Societario> Societarios => Set<Societario>();
        public DbSet<DadoPessoal> DadosPessoais => Set<DadoPessoal>();
        public DbSet<Cobranca> Cobrancas => Set<Cobranca>();
        public DbSet<Comprovante> Comprovantes => Set<Comprovante>();
        public DbSet<TransferenciaBancaria> Transferencias => Set<TransferenciaBancaria>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuditoriaConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClienteConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CobrancaConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ComprovanteConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ComplianceConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContaConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContatoConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DadoPessoalConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentoFiscalConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnderecoFiscalConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SocietarioConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransferenciaConfiguration).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.Entity is not Auditoria)
                .ToList();

            var result = base.SaveChanges();

            if (entities.Count > 0)
            {
                foreach (var e in entities)
                {
                    var tipoModificacao = AcaoAuditoria.OTHER;

                    tipoModificacao = e.State switch
                    {
                        EntityState.Detached => AcaoAuditoria.OTHER,
                        EntityState.Unchanged => AcaoAuditoria.STATUS,
                        EntityState.Deleted => AcaoAuditoria.DELETE,
                        EntityState.Modified => AcaoAuditoria.UPDATE,
                        EntityState.Added => AcaoAuditoria.INSERT,
                        _ => AcaoAuditoria.OTHER
                    };

                    Auditorias.Add(BuildAudit(e, tipoModificacao));
                }

                base.SaveChanges();
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var entities = ChangeTracker.Entries()
                .Where(e => e.Entity is not Auditoria)
                .ToList();

            var result = await base.SaveChangesAsync(ct);

            if (entities.Count > 0)
            {
                foreach (var e in entities)
                {
                    var tipoModificacao = AcaoAuditoria.OTHER;

                    tipoModificacao = e.State switch
                    {
                        EntityState.Detached => AcaoAuditoria.OTHER,
                        EntityState.Unchanged => AcaoAuditoria.STATUS,
                        EntityState.Deleted => AcaoAuditoria.DELETE,
                        EntityState.Modified => AcaoAuditoria.UPDATE,
                        EntityState.Added => AcaoAuditoria.INSERT,
                        _ => AcaoAuditoria.OTHER
                    };

                    Auditorias.Add(BuildAudit(e, tipoModificacao));
                }

                await base.SaveChangesAsync(ct);
            }

            return result;
        }

        private Auditoria BuildAudit(EntityEntry e, AcaoAuditoria acao)
        {
            var pk = string.Join(",",
                e.Properties.Where(p => p.Metadata.IsPrimaryKey())
                            .Select(p => p.CurrentValue?.ToString()));

            return new Auditoria
            {
                Entidade = e.Entity.GetType().Name,
                EntidadeId = pk,
                Acao = acao,
                Sucesso = true,
                UsuarioId = _user?.GetUserId(),
                UsuarioLogin = _user?.Name,
                CorrelacaoId = _user?.GetCorrelationId(),
                OrigemIp = _user?.GetIpAddress(),
                UserAgent = _user?.GetUserAgent(),
                DadosAntes = null,
                DadosDepois = ToJson(e.Entity)
            };
        }

        private static string? ToJson(object obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj, new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                    WriteIndented = false
                });
            }
            catch { return null; }
        }
    }
}
