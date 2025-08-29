using FHT.Api.Middleware;
using FHT.Infra.Data.Authorization;
using FHT.Infra.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace FHT.Api.Config
{
    public static class Abstractions
    {
        public static IServiceCollection AddAspNetUserConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var tipo = configuration.GetSection("Database:Provider").Value;
            var conn = configuration.GetConnectionString(tipo) ?? throw new InvalidOperationException($"ConnectionStrings:{tipo} não configurada.");
            var migrationsAsm = configuration["Database:MigrationsAssembly"];

            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseSqlServer(conn, sql =>
                {
                    if (!string.IsNullOrWhiteSpace(migrationsAsm))
                        sql.MigrationsAssembly(migrationsAsm);

                    sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                    sql.CommandTimeout(60);

                    sql.MigrationsHistoryTable("HISTORICO_MIGRACAO", "dbo");
                });

#if DEBUG
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                options.EnableSensitiveDataLogging();
#endif
            });

            return services;
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
