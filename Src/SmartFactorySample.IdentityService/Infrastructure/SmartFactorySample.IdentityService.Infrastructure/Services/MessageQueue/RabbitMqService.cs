using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IModel = RabbitMQ.Client.IModel;

namespace SmartFactorySample.IdentityService.Infrastructure.Services.MessageQueue
{
    public class RabbitMqService : IMessageQueueService
    {
        #region Dependencies
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        #endregion

        #region Constructor
        public RabbitMqService(IConfiguration configuration)
        {
            var factory = new ConnectionFactory { HostName = configuration["RabbitMq:HostName"] };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = configuration["RabbitMq:QueueName"];

            _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }
        #endregion

        #region Public Methods
        public async Task PublishAsync<T>(T message)
        {
            var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublish("", _queueName, null, msg);
        }

        public async Task<T> ConsumeAsync<T>()
        {
            var result = _channel.BasicGet(_queueName, true);
            return result != null ? JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(result.Body.ToArray())) : default;
        }
        #endregion
    }
}
