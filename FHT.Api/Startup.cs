using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FHT.Api.Config;
using FHT.Api.Config.IoC;
using FHT.Api.Config.Jwt;
using FHT.Api.Core.Config;
using FHT.Api.Middleware;
using FHT.Application.Mapping;
using FHT.Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.IO.Compression;
using System.Reflection;

namespace FHT.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHealthChecks();

            services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

            services.AddSwaggerConfiguration();

            services.AddCors(o =>
            {
                o.AddPolicy("Everything", p =>
                    p.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .WithExposedHeaders("X-Correlation-Id", "X-RateLimit-Limit", "X-RateLimit-Remaining", "Retry-After"));
            });

            services.AddCustomDbContext(Configuration);
            services.AddJwtConfiguration(Configuration, "AppSettings");
            services.AddAspNetUserConfiguration();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    Assembly.GetExecutingAssembly()
                );
            });

            services.AddDependencyInjectionConfiguration();
            services.AddMemoryCache();

            services.AddResponseCompression(o =>
            {
                o.EnableForHttps = true;
                o.Providers.Add<BrotliCompressionProvider>();
                o.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest);
            services.Configure<GzipCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest);
            services.Configure<AppJwtSettings>(Configuration.GetSection("JwtConfiguracao"));

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddAutoMapper(cfg => cfg.AllowNullCollections = true,
                typeof(DomainToDtoProfile).Assembly,
                typeof(DtoToDomainProfile).Assembly);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new MediatorModule());
            builder.RegisterModule(new ApplicationModule());
            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwaggerConfiguration(Configuration);

            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Path == "/")
                {
                    ctx.Response.Redirect("/swagger", permanent: false);
                    return;
                }
                await next();
            });

            app.UseHealthChecks("/healthz");

            app.ConfigureCustomExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseResponseCompression();

            app.UseRouting();

            app.UseCors("Everything");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RateLimitMiddleware>();

            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health").AllowAnonymous();
            });

            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Migrations");
            app.ApplicationServices.ApplyMigrationsWithAudit(logger);
        }
    }
}
