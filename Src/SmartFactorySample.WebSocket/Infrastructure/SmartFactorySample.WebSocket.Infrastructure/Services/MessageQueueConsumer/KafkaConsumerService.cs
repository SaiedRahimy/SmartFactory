using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFactorySample.WebSocket.Application.Common.Interfaces;
using SmartFactorySample.WebSocket.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Infrastructure.Services.MessageQueue
{
    public class KafkaConsumerService : IMessageQueueConsumerService
    {
        #region Properties

        private bool _isCancelled;
        private Task _service;

        #endregion

        #region Dependencies
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;
        private readonly IConsumer<Null, string> _consumer;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly ILocalBufferService _localBufferService;

        #endregion

        #region Constructor
        public KafkaConsumerService(ILogger<KafkaConsumerService> logger,IConfiguration configuration, ILocalBufferService localBufferService)
        {
            _logger = logger;
            _localBufferService = localBufferService;

            var config = new ProducerConfig { BootstrapServers = configuration["ConsumerKafka:BootstrapServers"] };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = configuration["ConsumerKafka:Topic"];

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["ConsumerKafka:BootstrapServers"],
                GroupId = configuration["ConsumerKafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
            _consumer.Subscribe(_topic);
        }
        #endregion

        #region Public Methods

        public void ConsumeData()
        {
            while (_isCancelled == false)
            {
                try
                {
                    var consumeResult = _consumer.Consume();
                    var data= JsonConvert.DeserializeObject<TagInfoDto>(consumeResult.Message.Value);
                    _localBufferService.Push(data);


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
