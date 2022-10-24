using BrunoMelo.FluxoCaixa.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using NuvTools.Data.EntityFrameworkCore.SqlServer.Extensions;
using NuvTools.Common.Configuration;
using System.Text.Json.Serialization;
using NuvTools.Common.Mail;
using NuvTools.Common.Dates;
using BrunoMelo.FluxoCaixa.Services.Security;
using BrunoMelo.FluxoCaixa.Models.Data.Seguranca;
using BrunoMelo.FluxoCaixa.Services.Operacional;
using BrunoMelo.FluxoCaixa.Models.Security;
using NuvTools.Common.ResultWrapper;
using BrunoMelo.FluxoCaixa.Services.Manutencao;
using Microsoft.AspNetCore.Builder;
using System.Globalization;
using BrunoMelo.FluxoCaixa.Services.Apoio;
using NuvTools.Security.AspNetCore.Services;
using NuvTools.Security.Extensions;
using NuvTools.Security.Identity.Policy;

namespace BrunoMelo.FluxoCaixa.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguration(configuration);
        services.AddDatabaseByConnectionName<Contexto>(configuration, "Principal").AddTransient<ContextoSeeder>();
        services.AddMvcCore().AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
        services.AddCors(configuration);
        services.AddSecurity(configuration);
        services.AddSharedInfrastructure();
        services.AddApplicationServices();
        services.RegisterSwagger();

        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
            //By default the below will be set to whatever the server culture is. 
            options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-BR") };
        });

        return services;
    }

    private static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        var appConfiguration = configuration.GetAppConfiguration();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                              builder =>
                              {
                                  builder.WithOrigins(appConfiguration.CORS)
                                              .AllowAnyHeader()
                                              .AllowAnyMethod();
                              });
        });

        return services;
    }

    private static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.IncludeXmlComments(string.Format(@"{0}\BrunoMelo.FluxoCaixa.API.xml", AppDomain.CurrentDomain.BaseDirectory));
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "BrunoMelo.FluxoCaixa" 
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    }, new List<string>()
                },
            });
            c.CustomSchemaIds(x => x.FullName);
        });
    }

    private static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IAuthorizationPolicyProvider, AuthorizationPermissionPolicyProvider>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
            .AddIdentity<User, Role>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<Contexto>()
            .AddDefaultTokenProviders();

        services.AddJwtAuthentication(configuration.GetAppConfiguration().Security);

        return services;
    }

    private static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeService, SystemDateTimeService>();
        services.AddTransient<SMTPMailService>();
        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //Segurança
        services.AddTransient<IdentityService>();
        services.AddTransient<RoleService>();
        services.AddTransient<UserService>();

        //Funcionalidades
        services.AddTransient<CategoriaService>();
        services.AddTransient<TipoTransacaoService>();
        services.AddTransient<ContaService>();
        services.AddTransient<TransacaoService>();

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services, Security config)
    {
        var key = Encoding.ASCII.GetBytes(config.SecretKey);
        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RoleClaimType = ClaimTypes.Role,
                    ClockSkew = TimeSpan.Zero
                };
                bearer.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(Result.Fail("You are not Authorized."));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(Result.Fail("You are not authorized to access this resource."));
                        return context.Response.WriteAsync(result);
                    },
                };
            });

        services.AddAuthorization(options =>
        {
            foreach (var item in Permissions.GetAllPermissions())
            {
                options.AddPolicyWithRequiredPermissionClaim(item.Value, item.Value);
            }
        });
        
        services.AddHttpContextAccessor();
        services.AddScoped<CurrentUserService>();
        return services;
    }
}