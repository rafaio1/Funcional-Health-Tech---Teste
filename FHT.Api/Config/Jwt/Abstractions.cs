﻿using FHT.Infra.Seguranca.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace FHT.Api.Config.Jwt
{
    public static class Abstractions
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration, string appJwtSettingsKey = null)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentException(nameof(configuration));
            }

            IConfigurationSection jwtConfiguration = configuration.GetSection("JwtConfiguracao");

            services.Configure<ConfiguracaoJwt>(jwtConfiguration);

            ConfiguracaoJwt appSettings = jwtConfiguration.Get<ConfiguracaoJwt>();

            byte[] key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.Audience,
                    ValidIssuer = appSettings.Issuer
                };
            });

            return services;
        }
    }
}
