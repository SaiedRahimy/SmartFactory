using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFactorySample.DataPresentation.Application.Common.Interfaces;
using SmartFactorySample.DataPresentation.Application.Dtos;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFactorySample.DataPresentation.Infrastructure.Services.Cache
{
    public class RedisProvider : IRedisProvider
    {

        #region Properties

        private static string prefixSet = "DataPresentation_";
        private static string tagKey = "Tag_";

        #endregion

        #region Dependencies
        private IDatabase _db;

        private readonly ILogger<RedisProvider> _logger;

        #endregion
        #region Constructor
        public RedisProvider(IConfiguration configuration, ILogger<RedisProvider> logger)
        {
            _logger = logger;
            var options = new ConfigurationOptions
            {
                EndPoints = { configuration["RedisUrl"] },
                ConnectRetry = 50_000,
                ReconnectRetryPolicy = new ExponentialRetry(5000, 20_000),
                ConnectTimeout = 1000,
                DefaultDatabase = 0,
            };

            _logger.LogInformation("Connecting to redis");
            var redisMultiplexer = ConnectionMultiplexer.Connect(options);
            _db = redisMultiplexer.GetDatabase();
        }
        #endregion

        #region Public Methods
        public Task SetTagsLastState(List<TagInfoDto> list)
        {
            var JsonData = JsonConvert.SerializeObject(list);
            _db.StringSet($"{prefixSet}{tagKey}_TagInfo", JsonData);
            _logger.LogInformation("Set Data into Redis");

            return Task.CompletedTask;
        }

        public async Task<List<TagInfoDto>> GetTagsLastState()
        {
            try
            {

                var value = await _db.StringGetAsync($"{prefixSet}_TagInfo");
                if (!string.IsNullOrEmpty(value))
                {
                    _logger.LogInformation("Get Data From Redis");

                    return JsonConvert.DeserializeObject<List<TagInfoDto>>(value);
                }
                else
                {
                    return new List<TagInfoDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Can Not Get Data From Redis", ex);

                return new List<TagInfoDto>();
            }
        }
        #endregion

    }
}
