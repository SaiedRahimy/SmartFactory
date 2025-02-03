using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartFactorySample.DataReception.Application.Common.Interfaces;
using SmartFactorySample.DataReception.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.Infrastructure.Services
{
    public class ConsumerHandler : IConsumerHandler
    {
        #region Dependencies
        private readonly ILogger<ConsumerHandler> _logger;
        private readonly IMessageQueueService _messageQueueService;

        #endregion

        #region Properties

        private bool _isRunning = true;
       

        #endregion

        #region Constructor
        public ConsumerHandler(IMessageQueueService messageQueueService, ILogger<ConsumerHandler> logger, IConfiguration configuration)
        {
            _logger = logger;
            _messageQueueService = messageQueueService;

        }
        #endregion

        #region Public Methods
        public async Task StartAsync()
        {
            _logger.LogInformation("SimulatorHandler started.");



        }

        public async Task StopAsync()
        {
            _logger.LogInformation("SimulatorHandler stopping...");
            _isRunning = false;
        }

        #endregion


        #region Private Methods

       



        #endregion
    }
}
