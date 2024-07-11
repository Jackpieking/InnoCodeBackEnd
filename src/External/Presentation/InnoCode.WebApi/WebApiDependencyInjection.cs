using System;
using System.Security.Cryptography;
using System.Text;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using InnoCode.Application.Share.Common;
using InnoCode.Configuration.Infrastructure.Mail.GoogleGmail;
using InnoCode.Configuration.Presentation.WebApi.Authentication;
using InnoCode.Configuration.Presentation.WebApi.CORS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using ODour.Configuration.Presentation.WebApi.Swagger;

namespace InnoCode.WebApi;

public static class WebApiDependencyInjection
{
    public static IServiceCollection Config(
        IServiceCollection services,
        IConfiguration configuration
    )
    {
        // ====
        var authOption = configuration
            .GetRequiredSection("Authentication")
            .Get<JwtAuthenticationOption>();

        TokenValidationParameters validationParameters =
            new()
            {
                ValidateIssuer = authOption.Jwt.ValidateIssuer,
                ValidateAudience = authOption.Jwt.ValidateAudience,
                ValidateLifetime = authOption.Jwt.ValidateLifetime,
                ValidateIssuerSigningKey = authOption.Jwt.ValidateIssuerSigningKey,
                RequireExpirationTime = authOption.Jwt.RequireExpirationTime,
                ValidTypes = authOption.Jwt.ValidTypes,
                ValidIssuer = authOption.Jwt.ValidIssuer,
                ValidAudience = authOption.Jwt.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    new HMACSHA256(Encoding.UTF8.GetBytes(authOption.Jwt.IssuerSigningKey)).Key
                )
            };

        services.AddAuthenticationJwtBearer(
            jwtSigningOption =>
            {
                jwtSigningOption.SigningKey = authOption.Jwt.IssuerSigningKey;
            },
            jwtBearerOption =>
            {
                jwtBearerOption.TokenValidationParameters = validationParameters;
                jwtBearerOption.Validate();
            }
        );

        // ====
        services.AddAuthorization();

        // ====
        var corsOption = configuration
            .GetRequiredSection("Cors")
            .GetRequiredSection("Default")
            .Get<CORSOption>();

        services.AddCors(config =>
        {
            config.AddDefaultPolicy(policy =>
            {
                // Origins.
                if (corsOption.Origins.Length == default)
                {
                    policy = policy.AllowAnyOrigin();
                }
                else
                {
                    policy = policy.WithOrigins(corsOption.Origins);
                }

                // Headers.
                if (corsOption.Headers.Length == default)
                {
                    policy = policy.AllowAnyHeader();
                }
                else
                {
                    policy = policy.WithHeaders(corsOption.Headers);
                }

                // Methods
                if (corsOption.Methods.Length == default)
                {
                    policy = policy.AllowAnyMethod();
                }
                else
                {
                    policy = policy.WithMethods(corsOption.Methods);
                }

                // Allow credentials.
                if (corsOption.AreCredentialsAllowed)
                {
                    policy = policy.AllowCredentials();
                }
            });
        });

        // ====
        services.AddDataProtection();

        // ====
        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddConsole();
        });

        // ====
        services.AddResponseCaching();

        // ====
        var swaggerOption = configuration
            .GetRequiredSection("Swagger")
            .GetRequiredSection("NSwag")
            .Get<NSwagOption>();

        services.SwaggerDocument(documentOption =>
        {
            documentOption.DocumentSettings = setting =>
            {
                setting.PostProcess = document =>
                {
                    document.Info = new()
                    {
                        Version = swaggerOption.Doc.PostProcess.Info.Version,
                        Title = swaggerOption.Doc.PostProcess.Info.Title,
                        Description = swaggerOption.Doc.PostProcess.Info.Description,
                        Contact = new()
                        {
                            Name = swaggerOption.Doc.PostProcess.Info.Contact.Name,
                            Email = swaggerOption.Doc.PostProcess.Info.Contact.Email
                        },
                        License = new()
                        {
                            Name = swaggerOption.Doc.PostProcess.Info.License.Name,
                            Url = new(swaggerOption.Doc.PostProcess.Info.License.Url)
                        }
                    };
                };

                setting.AddAuth(
                    JwtBearerDefaults.AuthenticationScheme,
                    new()
                    {
                        Type = (OpenApiSecuritySchemeType)
                            Enum.ToObject(
                                typeof(OpenApiSecuritySchemeType),
                                swaggerOption.Doc.Auth.Bearer.Type
                            ),
                        In = (OpenApiSecurityApiKeyLocation)
                            Enum.ToObject(
                                typeof(OpenApiSecurityApiKeyLocation),
                                swaggerOption.Doc.Auth.Bearer.In
                            ),
                        Scheme = swaggerOption.Doc.Auth.Bearer.Scheme,
                        BearerFormat = swaggerOption.Doc.Auth.Bearer.BearerFormat,
                        Description = swaggerOption.Doc.Auth.Bearer.Description,
                    }
                );
            };

            documentOption.EnableJWTBearerAuth = swaggerOption.EnableJWTBearerAuth;
        });

        // ====
        services.AddHttpContextAccessor();

        // ====
        #region Configs
        services
            // ====
            .AddSingleton(
                configuration
                    .GetRequiredSection("SmtpServer")
                    .GetRequiredSection("GoogleGmail")
                    .Get<GoogleSmtpServerOption>()
            )
            .MakeSingletonLazy<GoogleSmtpServerOption>();

        // ====
        services.AddSingleton(validationParameters).MakeSingletonLazy<TokenValidationParameters>();

        // ====
        services.AddSingleton(authOption).MakeSingletonLazy<JwtAuthenticationOption>();
        #endregion

        #region CustomServices
        services.MakeSingletonLazy<IServiceScopeFactory>();

        // ====
        services.MakeSingletonLazy<IHttpContextAccessor>();
        #endregion

        return services;
    }
}
