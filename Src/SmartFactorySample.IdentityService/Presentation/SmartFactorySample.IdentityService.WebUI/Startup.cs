using SmartFactorySample.IdentityService.Application;
using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using SmartFactorySample.IdentityService.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartFactorySample.IdentityService.WebUI.Filters;
using SmartFactorySample.IdentityService.WebUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFactorySample.IdentityService.Infrastructure.Services.MessageQueue;
using SmartFactorySample.IdentityService.Infrastructure.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using SmartFactorySample.IdentityService.Infrastructure.Identity;

namespace SmartFactorySample.IdentityService.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartFactorySample.IdentityService.WebUI", Version = "v1" });
            });

            services.AddApplication();
            services.AddInfrastructure(Configuration);

            // Validation exceptions
            services.AddControllersWithViews(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
                    .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            var provider = Configuration["MessageQueueProvider"];
            switch (provider)
            {

                case "Kafka":
                    services.AddSingleton<IMessageQueueService, KafkaService>();
                    break;

                case "RabbitMq":
                    services.AddSingleton<IMessageQueueService, RabbitMqService>();
                    break;

                default: throw new Exception("Invalid message queue provider");
            };


            services.AddSingleton<ISimulatorHandler, SimulatorHandler>();
            services.AddHostedService<ProcessorHostedService>();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartFactorySample.IdentityService.WebUI v1"));
            }

            app.UseRouting();

            app.UseExceptionHandler();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var user = new ApplicationUser { UserName = "admin", Email = "admin@example.com", EmailConfirmed = true };
                if (userManager.FindByNameAsync(user.UserName).Result == null)
                {
                    userManager.CreateAsync(user, "1234").Wait();
                    userManager.AddClaimsAsync(user, new List<Claim> { new Claim(JwtClaimTypes.Role, "admin") }).Wait();
                }
            }
        }
    }
}
