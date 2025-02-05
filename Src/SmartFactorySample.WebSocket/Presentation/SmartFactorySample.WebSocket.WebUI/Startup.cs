using SmartFactorySample.WebSocket.Application;
using SmartFactorySample.WebSocket.Application.Common.Interfaces;
using SmartFactorySample.WebSocket.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SmartFactorySample.WebSocket.WebUI.Filters;
using SmartFactorySample.WebSocket.WebUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartFactorySample.WebSocket.Infrastructure.Services.MessageQueue;
using SmartFactorySample.WebSocket.Infrastructure.Services;
using SmartFactorySample.WebSocket.Infrastructure.Services.Cache;
using SmartFactorySample.WebSocket.Infrastructure.Hubs;

namespace SmartFactorySample.WebSocket.WebUI
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartFactorySample.WebSocket.WebUI", Version = "v1" });
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartFactorySample.WebSocket.WebUI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<WebSocketHub>("/dataStream", conf =>
                {
                    conf.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                });
                endpoints.MapControllers();
            });
        }
    }
}
