using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using SmartFactorySample.DataReception.Application.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Infrastructure.Services
{
    public class LocalBufferService : ILocalBufferService
    {
        #region Dependencies
        private ConcurrentQueue<TagInfoDto> LocalBuffer;
        public static readonly PriorityQueue<TagInfoDto, DateTime> DbContextOutputCache = new PriorityQueue<TagInfoDto, DateTime>();

        #endregion

        #region Dependencies
        private readonly ILogger<LocalBufferService> _logger;
        private readonly ITagInfoManager _tagInfoManager;
        private readonly IMessageQueueService _messageQueueService;

        #endregion

        #region Properties


        private System.Timers.Timer TimerProcessData { get; set; }
        private System.Timers.Timer TimerProcessOutPutData { get; set; }

        #endregion

        #region Constructor
        public LocalBufferService(ILogger<LocalBufferService> logger, ITagInfoManager tagInfoManager, IMessageQueueService messageQueueService)
        {
            _logger = logger;
            _tagInfoManager = tagInfoManager;
            LocalBuffer = new ConcurrentQueue<TagInfoDto>();
            _messageQueueService = messageQueueService;


            

            StartTimerProcessData();
            StartTimerSendOutputData();
        }
        #endregion

        #region Public Methods

        public void Push(TagInfoDto tag)
        {
            LocalBuffer.Enqueue(tag);


        }


        #endregion

        #region private Methods


        private List<TagInfoDto> TakeFromLocalCache(int listCount)
        {

            var tags = new List<TagInfoDto>();

            if (LocalBuffer.Count > 0)
            {
                var count = Math.Min(listCount, LocalBuffer.Count);
                while (count >= 0 && LocalBuffer.Count > 0)
                {
                    if (LocalBuffer.TryDequeue(out TagInfoDto dto))
                    {
                        count--;
                        tags.Add(dto);
                    }
                }

            }
            return tags;
        }

        private async Task ProcessData(List<TagInfoDto> tags)
        {
            if (tags?.Count > 0)
            {
                try
                {
                    await SyncWithDatabase(tags);
                    await EnrichByPublishedSensorId(tags);
                    await PushTagsToOutPutQueue(tags);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception in ProcessLocalQueueData", ex);
                }
            }
        }

        private async Task SyncWithDatabase(List<TagInfoDto> tags)
        {
            await _tagInfoManager.RegisterNewTags(tags);
        }
        private async Task EnrichByPublishedSensorId(List<TagInfoDto> tags)
        {
            Parallel.ForEach(tags, new ParallelOptions { MaxDegreeOfParallelism = 2 }, tag =>
            {
                _tagInfoManager.EnrichTagId(tag);
            });

            await Task.CompletedTask;
        }


        private async Task PushTagsToOutPutQueue(List<TagInfoDto> tags)
        {
            if (tags.Any())
            {
                foreach (var tag in tags)
                {
                    DbContextOutputCache.Enqueue(tag, tag.DateTime);
                }

            }

            await Task.CompletedTask;
        }
        private List<TagInfoDto> GetAndRemoveTagsValue(int count = 1)
        {
            var result = new List<TagInfoDto>();
            while (count > 0 && DbContextOutputCache.Count > 0)
            {
                var getData = DbContextOutputCache.TryDequeue(out TagInfoDto data, out DateTime time);
                if (getData)
                {
                    result.Add(data);
                    count--;
                }
            }
            return result;
        }
        List<TagInfoDto> TakeFromLocalOutputCache(int howMuch, int min)
        {
            var dataCount = DbContextOutputCache.Count;
            var localList = new List<TagInfoDto>();

            if (dataCount > min)
            {
                int count = Math.Min(dataCount, howMuch);
                localList = GetAndRemoveTagsValue(count);

            }
            return localList;
        }

        void StartTimerProcessData()
        {
            TimerProcessData = new System.Timers.Timer(500);
            TimerProcessData.Elapsed += async (a, b) =>
            {
                if (LocalBuffer.Count > 0)
                {
                    var dataList = TakeFromLocalCache(5000);

                    await ProcessData(dataList);

                }
            };
            TimerProcessData.Enabled = true;
        }
        void StartTimerSendOutputData()
        {
            TimerProcessOutPutData = new System.Timers.Timer(1000);
            TimerProcessOutPutData.Enabled = true;
            TimerProcessOutPutData.Elapsed += (a, b) =>
            {

                var pack1 = TakeFromLocalOutputCache(2000, 0);
                var pack2 = TakeFromLocalOutputCache(2000, 200);
                var pack3 = TakeFromLocalOutputCache(2000, 200);

                if (pack1 is not null && pack1.Count > 0)
                {
                    Task.Run(async () => { await PublishOutputData(pack1); });
                }

                if (pack2 is not null && pack2.Count > 0)
                {
                    Task.Run(async () => { await PublishOutputData(pack2); });
                }

                if (pack3 is not null && pack3.Count > 0)
                {
                    Task.Run(async () => { await PublishOutputData(pack3); });
                }

            };
        }
        private async Task PublishOutputData(List<TagInfoDto> dtosToProcess)
        {
            try
            {
                await _messageQueueService.PublishAsync(dtosToProcess);
            }
            catch (Exception ex)
            {
                _logger.LogError($" Exception in TagsValue Archive Business: {ex.Message}", ex);
            }

        }
        #endregion
    }
}
