using SmartFactorySample.Simulator.Application;
using SmartFactorySample.Simulator.Application.Common.Interfaces;
using SmartFactorySample.Simulator.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartFactorySample.Simulator.WebUI.Filters;
using SmartFactorySample.Simulator.WebUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFactorySample.Simulator.Infrastructure.Services.MessageQueue;
using SmartFactorySample.Simulator.Infrastructure.Services;

namespace SmartFactorySample.Simulator.WebUI
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartFactorySample.Simulator.WebUI", Version = "v1" });
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartFactorySample.Simulator.WebUI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
