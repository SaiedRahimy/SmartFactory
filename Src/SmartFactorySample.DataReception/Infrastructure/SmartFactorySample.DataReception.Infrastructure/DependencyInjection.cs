using SmartFactorySample.DataReception.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFactorySample.DataReception.Infrastructure.Identity;
using SmartFactorySample.DataReception.Infrastructure.Persistence;
using SmartFactorySample.DataReception.Infrastructure.Persistence.Example;
using SmartFactorySample.DataReception.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SmartFactorySample.DataReception.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITagInfoManager, TagInfoManager>();


            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<DataReceptionContext>(options =>
                    options.UseInMemoryDatabase("SmartFactorySample.DataReceptionitectureDb"));
            }
            else
            {
                services.AddDbContext<DataReceptionContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(DataReceptionContext).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<DataReceptionContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataReceptionContext>();


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