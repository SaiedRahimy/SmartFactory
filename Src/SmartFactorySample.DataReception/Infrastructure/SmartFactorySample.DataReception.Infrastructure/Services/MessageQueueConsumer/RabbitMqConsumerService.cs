using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using SmartFactorySample.DataReception.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IModel = RabbitMQ.Client.IModel;

namespace SmartFactorySample.DataReception.Infrastructure.Services.MessageQueue
{
    public class RabbitMqConsumerService : IMessageQueueConsumerService
    {
        #region Properties

        private bool _isCancelled;
        private Task _service;

        #endregion

        #region Dependencies
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqConsumerService> _logger;
        private readonly ILocalBufferService _localBufferService;

        #endregion

        #region Constructor
        public RabbitMqConsumerService(ILogger<RabbitMqConsumerService> logger, IConfiguration configuration, ILocalBufferService localBufferService)
        {
            _logger = logger;
            _localBufferService = localBufferService;

            var factory = new ConnectionFactory { HostName = configuration["ConsumerRabbitMq:HostName"] };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = configuration["ConsumerRabbitMq:QueueName"];

            _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }
        #endregion

        #region Public Methods
        public void ConsumeData()
        {
            while (_isCancelled == false)
            {
                try
                {
                    var result = _channel.BasicGet(_queueName, true);
                    if (result != null)
                    {
                        var data = JsonConvert.DeserializeObject<TagInfoDto>(Encoding.UTF8.GetString(result.Body.ToArray()));
                        _localBufferService.Push(data);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            }

        }

        public void Stop()
        {
            _isCancelled = true;
        }

        #endregion
    }
}
