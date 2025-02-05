using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactorySample.IdentityService.Infrastructure.Identity;
using SmartFactorySample.IdentityService.Infrastructure.Persistence;
using SmartFactorySample.IdentityService.Infrastructure.Persistence.Example;
using SmartFactorySample.IdentityService.Infrastructure.Services;
using SmartFactorySample.IdentityService.Infrastructure.Identity;
using Confluent.Kafka;
using Duende.IdentityServer.Models;
using System.Collections.Generic;

namespace SmartFactorySample.IdentityService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddDeveloperSigningCredential();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, SmartFactorySample.IdentityService.Infrastructure.Identity.IdentityService>();


            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });

            return services;
        }
    }
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource> { new IdentityResources.OpenId(), new IdentityResources.Profile() };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope> { new ApiScope("SmartFactorySample") };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
            new Client
            {
                ClientId = "SmartFactorySample.DataPresentation",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "SmartFactorySample" }
            },
            new Client
            {
                ClientId = "SmartFactorySample.DataReception",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "SmartFactorySample" }
            },
            new Client
            {
                ClientId = "SmartFactorySample.Simulator",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "SmartFactorySample" }
            },
            new Client
            {
                ClientId = "SmartFactorySample.WebSocket",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "SmartFactorySample" }
            }
            };
    }

}