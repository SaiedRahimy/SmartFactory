using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartFactorySample.WebSocket.Application.Common.Interfaces;
using SmartFactorySample.WebSocket.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SmartFactorySample.WebSocket.Infrastructure.Services
{
    public class ConsumerHandler : IConsumerHandler
    {
        #region Dependencies
        private readonly ILogger<ConsumerHandler> _logger;
        private readonly IMessageQueueConsumerService _messageQueueConsumerService;

        #endregion

        #region Properties

        private bool _isRunning = true;


        #endregion

        #region Constructor
        public ConsumerHandler(IMessageQueueConsumerService messageQueueConsumerService, ILogger<ConsumerHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _messageQueueConsumerService = messageQueueConsumerService;

        }
        #endregion

        #region Public Methods
        public async Task StartAsync()
        {
            _logger.LogInformation("SimulatorHandler started.");

            await Task.Run(() => _messageQueueConsumerService.ConsumeData());

        }

        public async Task StopAsync()
        {
            _logger.LogInformation("SimulatorHandler stopping...");
            _messageQueueConsumerService.Stop();
            _isRunning = false;
        }

        #endregion


        #region Private Methods





        #endregion
    }
}
