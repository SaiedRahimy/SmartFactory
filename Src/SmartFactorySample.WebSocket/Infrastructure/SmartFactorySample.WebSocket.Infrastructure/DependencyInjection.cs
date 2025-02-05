using SmartFactorySample.WebSocket.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactorySample.WebSocket.Infrastructure.Identity;
using SmartFactorySample.WebSocket.Infrastructure.Persistence;
using SmartFactorySample.WebSocket.Infrastructure.Services;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using SmartFactorySample.WebSocket.Infrastructure.Hubs;

namespace SmartFactorySample.WebSocket.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {


            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<WebSocketContext>(options =>
                    options.UseInMemoryDatabase("SmartFactorySample.WebSocketitectureDb"));
            }
            else
            {
                services.AddDbContext<WebSocketContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(WebSocketContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<WebSocketContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<WebSocketContext>();


            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();




            var identityServerUrl = configuration["IdentityServerUrl"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = identityServerUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = "SmartFactorySample";
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (path.StartsWithSegments("/dataStream") && context.Request.Query.TryGetValue("access_token", out var accessToken))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                                context.HttpContext.Request.Headers.Add("Authorization", $"Bearer {accessToken}");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiAccess", policy => policy.RequireAuthenticatedUser());
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("SignalRAuthorization", policy => policy.Requirements.Add(new SignalRAuthorization()));
            });

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(5);
                options.KeepAliveInterval = TimeSpan.FromSeconds(5);
            });

            return services;
        }
    }
}