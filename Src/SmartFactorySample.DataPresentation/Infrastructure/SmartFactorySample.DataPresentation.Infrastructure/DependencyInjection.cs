using SmartFactorySample.DataPresentation.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactorySample.DataPresentation.Infrastructure.Identity;
using SmartFactorySample.DataPresentation.Infrastructure.Persistence;
using SmartFactorySample.DataPresentation.Infrastructure.Services;
using System;
using SmartFactorySample.DataPresentation.Infrastructure.Persistence.TagDailyData;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SmartFactorySample.DataPresentation.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITagDailyDataManager, TagDailyDataManager>();


            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<DataPresentationContext>(options =>
                    options.UseInMemoryDatabase("SmartFactorySample.DataPresentationitectureDb"));
            }
            else
            {
                services.AddDbContext<DataPresentationContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(DataPresentationContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<DataPresentationContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataPresentationContext>();


            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();


            var identityServerUrl = configuration["IdentityServerUrl"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = identityServerUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = "SmartFactorySample";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiAccess", policy => policy.RequireAuthenticatedUser());
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });


            return services;
        }
    }
}