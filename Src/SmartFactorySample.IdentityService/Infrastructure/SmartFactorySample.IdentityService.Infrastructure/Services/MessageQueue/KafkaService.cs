using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmartFactorySample.IdentityService.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.IdentityService.Infrastructure.Services.MessageQueue
{
    public class KafkaService : IMessageQueueService
    {
        #region Dependencies
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;
        private readonly IConsumer<Null, string> _consumer;
        #endregion

        #region Constructor
        public KafkaService(IConfiguration configuration)
        {
            var config = new ProducerConfig { BootstrapServers = configuration["Kafka:BootstrapServers"] };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = configuration["Kafka:Topic"];

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        }
        #endregion

        #region Public Methods
        public async Task PublishAsync<T>(T message)
        {
            var msg = JsonConvert.SerializeObject(message);
            await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = msg });
        }

        public async Task<T> ConsumeAsync<T>()
        {
            _consumer.Subscribe(_topic);
            var consumeResult = _consumer.Consume();
            return JsonConvert.DeserializeObject<T>(consumeResult.Message.Value);
        }
        #endregion
    }
}
