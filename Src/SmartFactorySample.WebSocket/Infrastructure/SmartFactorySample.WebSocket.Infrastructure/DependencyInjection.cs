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


          
            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
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