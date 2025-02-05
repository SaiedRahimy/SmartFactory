using SmartFactorySample.DataPresentation.Application;
using SmartFactorySample.DataPresentation.Application.Common.Interfaces;
using SmartFactorySample.DataPresentation.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartFactorySample.DataPresentation.WebUI.Filters;
using SmartFactorySample.DataPresentation.WebUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFactorySample.DataPresentation.Infrastructure.Services.MessageQueue;
using SmartFactorySample.DataPresentation.Infrastructure.Services;
using SmartFactorySample.DataPresentation.Infrastructure.Services.Cache;
using SmartFactorySample.DataPresentation.WebUI.Handler;

namespace SmartFactorySample.DataPresentation.WebUI
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartFactorySample.DataPresentation.WebUI", Version = "v1" });
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
                    services.AddSingleton<IMessageQueueConsumerService, KafkaConsumerService>();
                    break;

                case "RabbitMq":
                    services.AddSingleton<IMessageQueueConsumerService, RabbitMqConsumerService>();
                    break;

                default: throw new Exception("Invalid message queue provider");
            };


            services.AddSingleton<IRedisProvider, RedisProvider>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<ILocalBufferService, LocalBufferService>();
            services.AddSingleton<IConsumerHandler, ConsumerHandler>();
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartFactorySample.DataPresentation.WebUI v1"));
            }

            app.UseRouting();
            app.UseExceptionHandler();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapControllers();
            });
        }
    }
}
