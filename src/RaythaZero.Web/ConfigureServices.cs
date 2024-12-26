using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using RaythaZero.Application.Common.Interfaces;
using RaythaZero.Application.Common.Security;
using RaythaZero.Domain.Entities;
using RaythaZero.Web.Services;
using CSharpVitamins;
using RaythaZero.Web.Authentication;
using RaythaZero.Web.Middlewares;
using Microsoft.AspNetCore.DataProtection;
using Raytha.Web.Authentication;
using Raytha.Web.Services;
using RaythaZero.Infrastructure.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            options =>
            {
                options.LoginPath = new PathString("/admin/login-redirect");
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
                options.AccessDeniedPath = new PathString("/admin/403");
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.EventsType = typeof(CustomCookieAuthenticationEvents);
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(RaythaClaimTypes.IsAdmin,
                policy => policy.Requirements.Add(new IsAdminRequirement()));
            options.AddPolicy(BuiltInSystemPermission.MANAGE_USERS_PERMISSION,
                policy => policy.Requirements.Add(new ManageUsersRequirement()));
            options.AddPolicy(BuiltInSystemPermission.MANAGE_ADMINISTRATORS_PERMISSION,
                policy => policy.Requirements.Add(new ManageAdministratorsRequirement()));
            options.AddPolicy(BuiltInSystemPermission.MANAGE_AUDIT_LOGS_PERMISSION,
                policy => policy.Requirements.Add(new ManageAuditLogsRequirement()));
            options.AddPolicy(BuiltInSystemPermission.MANAGE_SYSTEM_SETTINGS_PERMISSION,
                policy => policy.Requirements.Add(new ManageSystemSettingsRequirement()));

            options.AddPolicy(RaythaApiAuthorizationHandler.POLICY_PREFIX + RaythaClaimTypes.IsAdmin,
                policy => policy.Requirements.Add(new ApiIsAdminRequirement()));
            options.AddPolicy(RaythaApiAuthorizationHandler.POLICY_PREFIX + BuiltInSystemPermission.MANAGE_SYSTEM_SETTINGS_PERMISSION,
                policy => policy.Requirements.Add(new ApiManageSystemSettingsRequirement()));
            options.AddPolicy(RaythaApiAuthorizationHandler.POLICY_PREFIX + BuiltInSystemPermission.MANAGE_USERS_PERMISSION,
                policy => policy.Requirements.Add(new ApiManageUsersRequirement()));
        });

        services.AddScoped<CustomCookieAuthenticationEvents>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ICurrentOrganization, CurrentOrganization>();
        services.AddScoped<IRelativeUrlBuilder, RelativeUrlBuilder>();
        services.AddScoped<IRenderEngine, RenderEngine>();
        services.AddSingleton<IFileStorageProviderSettings, FileStorageProviderSettings>();
        services.AddSingleton<ICurrentVersion, CurrentVersion>();

        services.AddScoped<IAuthorizationHandler, RaythaAdminAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, RaythaApiAuthorizationHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, ApiKeyAuthorizationMiddleware>();

        services.AddScoped<ICsvService, CsvService>();

        services.AddRouting();
        services.AddDataProtection().SetApplicationName("Raytha").PersistKeysToDbContext<RaythaDbContext>();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => {
            c.CustomSchemaIds(type => type.ToString());
            c.DocumentFilter<LowercaseDocumentFilter>();
            c.SchemaFilter<ExcludePropertyFromOpenApiDocsFilter>();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Raytha API - V1",
                Version = "v1"
            });

            c.MapType<ShortGuid>(() => new OpenApiSchema { Type = "string" });

            c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "X-API-KEY must appear in header",
                Type = SecuritySchemeType.ApiKey,
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });
            var key = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header
            };
            var requirement = new OpenApiSecurityRequirement
                    {
                             { key, new List<string>() }
                    };
            c.AddSecurityRequirement(requirement);
        });

        return services;
    }
}

