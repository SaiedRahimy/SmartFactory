using Azure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartFactorySample.WebSocket.Application.Common.Interfaces;
using SmartFactorySample.WebSocket.Application.Dtos;
using SmartFactorySample.WebSocket.Infrastructure.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Infrastructure.Services
{
    public class LocalBufferService : ILocalBufferService
    {
        #region Dependencies
        private ConcurrentQueue<TagInfoDto> LocalBuffer;

        #endregion

        #region Dependencies
        private readonly ILogger<LocalBufferService> _logger;
        private readonly ICacheService _cacheService;
        private readonly IHubContext<WebSocketHub, IWebSocketHub> _webSocketHub;


        #endregion

        #region Properties


        private System.Timers.Timer TimerProcessData { get; set; }

        #endregion

        #region Constructor
        public LocalBufferService(ILogger<LocalBufferService> logger, ICacheService cacheService, IHubContext<WebSocketHub, IWebSocketHub> webSocketHub)
        {
            _logger = logger;
            _webSocketHub = webSocketHub;
            _cacheService = cacheService;
            LocalBuffer = new ConcurrentQueue<TagInfoDto>();

            StartTimerProcessData();
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
                    await SendDatas(tags);

                    _cacheService.SetData(tags);

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception in ProcessLocalQueueData", ex);
                }
            }
        }

        async Task SendDatas(List<TagInfoDto> tags)
        {
            foreach (var tag in tags)
            {
                var groupName = HubKeys.GenerateSensorGroupName(tag.Id);
                await _webSocketHub.Clients.Group(groupName).LiveData(tag);
            }
        }

        void StartTimerProcessData()
        {
            TimerProcessData = new System.Timers.Timer(500);
            TimerProcessData.Elapsed += async (a, b) =>
            {
                if (LocalBuffer.Count > 0)
                {
                    var dataList = TakeFromLocalCache(1000);

                    await ProcessData(dataList);

                }
            };
            TimerProcessData.Enabled = true;
        }

        #endregion
    }
}
