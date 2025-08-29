using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace FHT.Api.Config
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            string xmlComentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlComentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlComentsFile);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Funcional Health Tech",
                    Description = "API's para os devidos cadastros, atualizações e deleções de dados bancários.",
                    Contact = new OpenApiContact { Name = "Funcional Health Tech", Url = new Uri("https://www.funcionalhealthtech.com.br/") }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autorização JWT com o esquema Bearer no cabeçalho. \r\n\r\n Digite 'Bearer' [espaço] e o token que possui logo após.\r\n\r\nExemplo: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.OperationFilter<CorrelationIdHeaderOperationFilter>();

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                c.IncludeXmlComments(xmlComentsFullPath, true);

            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app, IConfiguration configuration)
        {
            var authPrefix = configuration["Auth"];

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FHT API v1");

                c.UseResponseInterceptor(@"
                (res) => {
                  try {
                    const json = typeof res.data === 'string' ? JSON.parse(res.data) : res.data;
                    if (json && json.access_token) { window.fhtJwt = json.access_token; }
                  } catch(e) {}
                  return res;
                }");

                c.UseRequestInterceptor($@"
                (req) => {{
                  try {{
                    const KEY = 'X-Correlation-Id';
                    if (!req.headers[KEY]) {{
                      const id = (crypto?.randomUUID?.() ?? (Date.now().toString(36)+Math.random().toString(36).slice(2))).replace(/-/g,'');
                      req.headers[KEY] = id.substring(0,32);
                    }}
                  }} catch(e) {{}}
                
                  const raw = (req.url || '').toLowerCase();
                  const url = raw.split('?')[0].replace(/\/+$/, '');
                  const isLogin = url.endsWith('/api/seguranca/login');
                
                  if (isLogin) {{
                    const stamp = new Date().toISOString().slice(0,10).replace(/-/g,''); 
                    const prefix = '{configuration["Auth"] ?? "Auth"}';
                    const plain  = `${{prefix}}:${{stamp}}:FHT`;
                
                    const toB64 = (s) => (typeof btoa === 'function' ? btoa(s) : Buffer.from(s,'utf8').toString('base64'));
                
                    req.headers['Authorization'] = 'Basic ' + toB64(plain);
                  }} else if (window.fhtJwt) {{
                    req.headers['Authorization'] = 'Bearer ' + window.fhtJwt;
                  }}
                
                  return req;
                }}");
                
            });

        }
    }
    public class CorrelationIdHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new System.Collections.Generic.List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = FHT.Api.Middleware.CorrelationIdMiddleware.HeaderName,
                In = ParameterLocation.Header,
                Required = false,
                Description = "Correlação de requisições (opcional; gerado automaticamente no Swagger)",
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}
