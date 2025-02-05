using Azure;
using Confluent.Kafka;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartFactorySample.WebSocket.Application.Common.Interfaces;
using SmartFactorySample.WebSocket.Application.Entities.TagInfo.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Infrastructure.Hubs
{
    public class WebSocketHub : Hub<IWebSocketHub>
    {
        #region Properties


        #endregion

        #region Dependencies

        private readonly ILogger<WebSocketHub> _logger;
        private readonly IMediator _mediator;

        #endregion

        #region Constructor
        public WebSocketHub(ILogger<WebSocketHub> logger, IConfiguration configuration, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;


        }
        #endregion

        #region Public Methods

        public override async Task OnConnectedAsync()
        {
            if (Context != null)
            {
                _logger.LogInformation($"Connect New Connection :{Context.ConnectionId} ");

            }

            await base.OnConnectedAsync();
        }


        public async Task RegisterTag(RegisterTagInfosCommand request)
        {
            var result = await _mediator.Send(request);

            foreach (var tag in result)
            {
                await Clients.Caller.LiveData(tag);

                var groupName = HubKeys.GenerateSensorGroupName(tag.Id);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }

        }

        public async Task UnRegisterSensor(UnRegisterTagInfosCommand model)
        {
            foreach (var id in model.TagIds)
            {
                var groupName = HubKeys.GenerateSensorGroupName(id);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }

            await Task.CompletedTask;
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context != null)
            {
                _logger.LogInformation($"Connection Closed, Connection :{Context.ConnectionId} ");
            }

            await base.OnDisconnectedAsync(exception);
        }
        #endregion

        
    }
}
