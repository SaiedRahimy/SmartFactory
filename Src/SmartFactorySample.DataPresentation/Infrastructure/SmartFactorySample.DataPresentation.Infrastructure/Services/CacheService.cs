using Azure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartFactorySample.DataPresentation.Application.Common.Interfaces;
using SmartFactorySample.DataPresentation.Application.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFactorySample.DataPresentation.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        #region Cache State
        private ConcurrentDictionary<int, TagInfoDto> _cache;

        #endregion

        #region Dependencies
        private readonly ILogger<CacheService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IRedisProvider _redisProvider;


        #endregion

        #region Properties


        private System.Timers.Timer TimerSyncRedis { get; set; }

        #endregion

        #region Constructor
        public CacheService(ILogger<CacheService> logger, IConfiguration configuration, IRedisProvider redisProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _redisProvider = redisProvider;
            _cache = new ConcurrentDictionary<int, TagInfoDto>();

            StartTimerSyncRedis();
        }
        #endregion

        #region Public Methods

        public void SetData(TagInfoDto tag)
        {
            _cache.AddOrUpdate(tag.Id, tag, (key, value) => tag);
        }

        public void SetData(List<TagInfoDto> tags)
        {
            foreach(var tag in tags)
            {
                _cache.AddOrUpdate(tag.Id, tag, (key, value) => tag);

            }
        }

        public TagInfoDto? GetData(int key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            return null;
        }

        public List<TagInfoDto> GetData(List<int> keys)
        {
            var list = new List<TagInfoDto>();
            foreach (int key in keys)
            {
                if (_cache.ContainsKey(key))
                {
                    list.Add(_cache[key]);
                }
            }
            return list;
        }

        #endregion

        #region private Methods


        private async Task FillCacheFromRedis()
        {
           var datas= await _redisProvider.GetTagsLastState();

            if(datas != null)
            {
                foreach (var tag in datas)
                {
                    _cache.AddOrUpdate(tag.Id, tag, (key, value) => tag);

                }
            }
        }



        void StartTimerSyncRedis()
        {
            var SyncRedisPeriod = int.Parse(_configuration["SyncRedisPeriod"]);
            TimerSyncRedis = new System.Timers.Timer(TimeSpan.FromMinutes(SyncRedisPeriod));
            TimerSyncRedis.Elapsed += async (a, b) =>
            {
              await  _redisProvider.SetTagsLastState(_cache.Values.ToList());
            };
            TimerSyncRedis.Enabled = true;
        }

        #endregion
    }
}
